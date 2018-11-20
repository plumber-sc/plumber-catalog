using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Plumber.Component.Commanders;
using Plugin.Plumber.Component.Extensions;

namespace Plugin.Plumber.Component.Pipelines.Blocks
{
    /// <summary>
    ///     Populates the actions for the specified component
    /// </summary>
    [PipelineDisplayName("PopulateComponentActionsBlock")]
    public class PopulateComponentActionsBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly ViewCommander viewCommander;
        private readonly CatalogSchemaCommander catalogSchemaCommander;        

        public PopulateComponentActionsBlock(ViewCommander viewCommander, CatalogSchemaCommander catalogSchemaCommander)
        {
            this.viewCommander = viewCommander;
            this.catalogSchemaCommander = catalogSchemaCommander;
        }

        public async override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{Name}: The argument cannot be null.");

            var request = this.viewCommander.CurrentEntityViewArgument(context.CommerceContext);

            var commerceEntity = request?.Entity;

            if (commerceEntity != null)
            {
                List<Type> applicableComponentTypes = await this.catalogSchemaCommander.GetApplicableComponentTypes(context.CommerceContext, commerceEntity);

                var editableComponentType = applicableComponentTypes.SingleOrDefault(type => type.FullName == arg.Name);

                if(editableComponentType != null)
                {
                    var entityViewAttribute = editableComponentType.GetEntityViewAttribute();

                    var actionPolicy = arg.GetPolicy<ActionsPolicy>();

                    actionPolicy.Actions.Add(new EntityActionView
                    {
                        Name = $"Edit-{editableComponentType.FullName}",
                        DisplayName = $"Edit {entityViewAttribute.ViewName}",
                        Description = entityViewAttribute.ViewName,
                        IsEnabled = true,
                        EntityView = arg.Name,
                        Icon = "edit"
                    });
                }
            }
            
            return arg;
        }
    }
}
