using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Plugin.Plumber.Catalog.Attributes;
using Plugin.Plumber.Catalog.Pipelines.Arguments;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Plumber.Catalog.Commanders;
using Plugin.Plumber.Catalog.Extensions;

namespace Plugin.Plumber.Catalog.Pipelines.Blocks
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

            if(!(request?.Entity is SellableItem))
            {
                return arg;
            }

            var sellableItem = (SellableItem)request.Entity;

            if (sellableItem != null)
            {
                List<Type> applicableComponentTypes = await this.catalogSchemaCommander.GetApplicableComponentTypes(context.CommerceContext, sellableItem);

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
