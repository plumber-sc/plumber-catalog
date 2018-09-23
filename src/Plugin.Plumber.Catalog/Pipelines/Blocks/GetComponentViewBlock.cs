using System;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using System.Linq;
using Plugin.Plumber.Catalog.Attributes;
using System.Collections.Generic;
using Plugin.Plumber.Catalog.Commanders;

namespace Plugin.Plumber.Catalog.Pipelines.Blocks
{
    /// <summary>
    ///     Creates the component view applicable to the selected sellable item.
    /// </summary>
    [PipelineDisplayName("GetComponentViewBlock")]
    public class GetComponentViewBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly ViewCommander viewCommander;
        private readonly CatalogSchemaCommander catalogSchemaCommander;

        public GetComponentViewBlock(ViewCommander viewCommander, CatalogSchemaCommander catalogSchemaCommander)
        {
            this.viewCommander = viewCommander;
            this.catalogSchemaCommander = catalogSchemaCommander;
        }

        public async override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{Name}: The argument cannot be null.");
            var request = this.viewCommander.CurrentEntityViewArgument(context.CommerceContext);

            // Only proceed if the current entity is a sellable item
            if (!(request.Entity is SellableItem))
            {
                return arg;
            }

            var sellableItem = (SellableItem)request.Entity;

            var catalogViewsPolicy = context.GetPolicy<KnownCatalogViewsPolicy>();
            var isCatalogView = request.ViewName.Equals(catalogViewsPolicy.Master, StringComparison.OrdinalIgnoreCase);
            var isVariationView = request.ViewName.Equals(catalogViewsPolicy.Variant, StringComparison.OrdinalIgnoreCase);
            var isPotentialEditView = arg.Action.StartsWith("Edit-", StringComparison.OrdinalIgnoreCase);
          
            // Make sure that we target the correct views
            if (!isCatalogView && !isVariationView && !isPotentialEditView)
            {
                return arg;
            }

            List<Type> applicableComponentTypes = await this.catalogSchemaCommander.GetApplicableComponentTypes(context.CommerceContext, sellableItem);

            // See if we are dealing with the base sellable item or one of its variations.
            var variationId = string.Empty;
            if (isVariationView && !string.IsNullOrEmpty(arg.ItemId))
            {
                variationId = arg.ItemId;
            }

            var targetView = arg;

            foreach (var componentType in applicableComponentTypes)
            {
                System.Attribute[] attrs = System.Attribute.GetCustomAttributes(componentType);
                
                var component = sellableItem.Components.SingleOrDefault(comp => comp.GetType() == componentType);

                if (attrs.SingleOrDefault(attr => attr is EntityViewAttribute) is EntityViewAttribute entityViewAttribute)
                {
                    // Check if the edit action was requested
                    var isEditView = !string.IsNullOrEmpty(arg.Action) && arg.Action.Equals($"Edit-{componentType.FullName}", StringComparison.OrdinalIgnoreCase);

                    if (!isEditView && !isPotentialEditView)
                    {
                        // Create a new view and add it to the current entity view.
                        var view = new EntityView
                        {
                            Name = componentType.FullName,
                            DisplayName = entityViewAttribute?.ViewName ?? componentType.Name,
                            EntityId = arg.EntityId,
                            EntityVersion = arg.EntityVersion,
                            ItemId = variationId
                        };

                        arg.ChildViews.Add(view);

                        targetView = view;
                    }
                    else
                    {
                        targetView.DisplayName = entityViewAttribute?.ViewName ?? componentType.Name;
                    }

                    if (isCatalogView || isVariationView || (isPotentialEditView && isEditView))
                    {
                        var props = componentType.GetProperties();

                        foreach (var prop in props)
                        {
                            System.Attribute[] propAttributes = System.Attribute.GetCustomAttributes(prop);

                            if (propAttributes.SingleOrDefault(attr => attr is PropertyAttribute) is PropertyAttribute propAttr)
                            {
                                if (isEditView || (!isEditView && propAttr.ShowInList))
                                {
                                    var viewProperty = new ViewProperty
                                    {
                                        Name = prop.Name,
                                        DisplayName = propAttr.DisplayName,
                                        RawValue = component != null ? prop.GetValue(component) : "",
                                        IsReadOnly = !isEditView && propAttr.IsReadOnly,
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