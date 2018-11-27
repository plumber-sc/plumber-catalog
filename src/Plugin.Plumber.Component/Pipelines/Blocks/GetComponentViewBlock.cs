using System;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using System.Linq;
using Plugin.Plumber.Component.Attributes;
using System.Collections.Generic;
using Plugin.Plumber.Component.Commanders;
using Sitecore.Commerce.Plugin.Promotions;
using Plugin.Plumber.Component.Pipelines.Arguments;

namespace Plugin.Plumber.Component.Pipelines.Blocks
{
    /// <summary>
    ///     Creates the component view applicable to the selected sellable item.
    /// </summary>
    [PipelineDisplayName("GetComponentViewBlock")]
    public class GetComponentViewBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly ViewCommander viewCommander;
        private readonly ComponentViewCommander commander;

        public GetComponentViewBlock(ViewCommander viewCommander, ComponentViewCommander catalogSchemaCommander)
        {
            this.viewCommander = viewCommander;
            this.commander = catalogSchemaCommander;
        }

        public async override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{Name}: The argument cannot be null.");
            var request = this.viewCommander.CurrentEntityViewArgument(context.CommerceContext);

            if(request.Entity == null)
            {
                return arg;
            }

            var entityViewConditionsArgument = new EntityViewConditionsArgument(request.Entity, request.ViewName, arg.Action);
            var result = await commander.Pipeline<IGetApplicableViewConditionsPipeline>().Run(entityViewConditionsArgument, context);

            // Only proceed if the current entity is supported
            if (!result.IsSupportedEntity)
            {
                return arg;
            }

            // Check if this is an edit view or display view
            if(!result.IsEditView && !result.IsDisplayView)
            {
                return arg;
            }

            List<Type> applicableComponentTypes = await this.commander.GetApplicableComponentTypes(request.Entity, request.ItemId, context.CommerceContext);

            var targetView = arg;

            foreach (var componentType in applicableComponentTypes)
            {
                System.Attribute[] attrs = System.Attribute.GetCustomAttributes(componentType);

                var component = request.Entity.Components.SingleOrDefault(comp => comp.GetType() == componentType);

                if (attrs.SingleOrDefault(attr => attr is EntityViewAttribute) is EntityViewAttribute entityViewAttribute)
                {
                    // Check if the edit action was requested for this specific component type
                    var isEditViewForThisComponent = !string.IsNullOrEmpty(arg.Action) && arg.Action.Equals($"Edit-{componentType.FullName}", StringComparison.OrdinalIgnoreCase);

                    if (result.IsDisplayView)
                    {
                        // Create a new view and add it to the current entity view.
                        var view = new EntityView
                        {
                            Name = componentType.FullName,
                            DisplayName = entityViewAttribute?.ViewName ?? componentType.Name,
                            EntityId = arg.EntityId,
                            EntityVersion = arg.EntityVersion
                        };

                        arg.ChildViews.Add(view);

                        targetView = view;
                    }
                    else
                    {
                        targetView.DisplayName = entityViewAttribute?.ViewName ?? componentType.Name;
                    }

                    if (result.IsDisplayView || (result.IsEditView && isEditViewForThisComponent))
                    {
                        var props = componentType.GetProperties();

                        foreach (var prop in props)
                        {
                            System.Attribute[] propAttributes = System.Attribute.GetCustomAttributes(prop);

                            if (propAttributes.SingleOrDefault(attr => attr is PropertyAttribute) is PropertyAttribute propAttr)
                            {
                                if (isEditViewForThisComponent || (!isEditViewForThisComponent && propAttr.ShowInList))
                                {
                                    var viewProperty = new ViewProperty
                                    {
                                        Name = prop.Name,
                                        DisplayName = propAttr.DisplayName,
                                        RawValue = component != null ? prop.GetValue(component) : "",
                                        IsReadOnly = !isEditViewForThisComponent && propAttr.IsReadOnly,
                                        IsRequired = propAttr.IsRequired
                                    };

                                    targetView.Properties.Add(viewProperty);
                                }
                            }
                        }
                    }
                }
            }

            return arg;
        }
    }
}