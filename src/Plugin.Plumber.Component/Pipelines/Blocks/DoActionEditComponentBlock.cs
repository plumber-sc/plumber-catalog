using System;
using System.Linq;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using Plugin.Plumber.Component.Commanders;

namespace Plugin.Plumber.Component.Pipelines.Blocks
{
    /// <summary>
    ///     Applies the entered data to the component that is being edited.
    /// </summary>
    [PipelineDisplayName(Constants.Pipelines.Blocks.DoActionEditComponentBlock)]
    public class DoActionEditComponentBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly ComponentViewCommander catalogSchemaCommander;

        public DoActionEditComponentBlock(ComponentViewCommander catalogSchemaCommander)
        {
            this.catalogSchemaCommander = catalogSchemaCommander;
        }

        public async override Task<EntityView> Run(EntityView entityView, CommercePipelineExecutionContext context)
        {
            Condition.Requires(entityView).IsNotNull($"{Name}: The argument cannot be null.");

            // Only proceed if the right action was invoked
            if (string.IsNullOrEmpty(entityView.Action) || !entityView.Action.StartsWith("Edit-", StringComparison.OrdinalIgnoreCase))
            {
                return entityView;
            }

            // Get the commerce entity from the context
            var commerceEntity = context.CommerceContext.GetObject<CommerceEntity>(x => x.Id.Equals(entityView.EntityId));
            if (commerceEntity == null)
            {
                return entityView;
            }

            KnownResultCodes errorCodes = context.CommerceContext.GetPolicy<KnownResultCodes>();
            if (context.CommerceContext.AnyMessage(msg => msg.Code == errorCodes.ValidationError))
            {   // We found an error
                return entityView;
            }

            var components = commerceEntity.Components;
            if(!string.IsNullOrWhiteSpace(entityView.ItemId) && commerceEntity is SellableItem)
            {
                var variation = ((SellableItem)commerceEntity).GetVariation(entityView.ItemId);
                if (variation != null)
                {
                    components = variation.ChildComponents;
                }
            }

            var allComponentTypes = await catalogSchemaCommander.GetAllComponentTypes(context.CommerceContext);
            var editedComponentType = allComponentTypes.SingleOrDefault(compType => entityView.Action == $"Edit-{compType.FullName}");
            var editedComponent = components.SingleOrDefault(comp => entityView.Action == $"Edit-{comp.GetType().FullName}");

            if(editedComponent == null)
            {
                editedComponent = (Sitecore.Commerce.Core.Component)Activator.CreateInstance(editedComponentType);
                components.Add(editedComponent);
            }
      
            if (editedComponent != null)
            {
                catalogSchemaCommander.SetPropertyValuesOnEditedComponent(entityView.Properties, editedComponent.GetType(), editedComponent, context.CommerceContext);

                // Persist changes
                await this.catalogSchemaCommander.Pipeline<IPersistEntityPipeline>().Run(new PersistEntityArgument(commerceEntity), context);
            }

            return entityView;
        }

    }
}
