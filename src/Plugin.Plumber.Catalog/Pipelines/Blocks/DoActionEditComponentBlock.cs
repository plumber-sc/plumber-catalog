using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using System;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Plumber.Catalog.Commanders;

namespace Plugin.Plumber.Catalog.Pipelines.Blocks
{
    /// <summary>
    ///     Applies the entered data to the component that is being edited.
    /// </summary>
    [PipelineDisplayName(Constants.Pipelines.Blocks.DoActionEditComponentBlock)]
    public class DoActionEditComponentBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly CatalogSchemaCommander catalogSchemaCommander;

        public DoActionEditComponentBlock(CatalogSchemaCommander catalogSchemaCommander)
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

            // Get the sellable item from the context
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

            var applicableComponentTypes = await this.catalogSchemaCommander.GetApplicableComponentTypes(context.CommerceContext, commerceEntity);
            var editedComponentType = applicableComponentTypes.SingleOrDefault(comp => entityView.Action == $"Edit-{comp.FullName}");

            if (editedComponentType != null)
            {
                // Get the component from the sellable item or its variation
                var editedComponent = catalogSchemaCommander.GetEditedComponent(commerceEntity, editedComponentType);

                catalogSchemaCommander.SetPropertyValuesOnEditedComponent(entityView.Properties, editedComponentType, editedComponent, context.CommerceContext);

                // Persist changes
                await this.catalogSchemaCommander.Pipeline<IPersistEntityPipeline>().Run(new PersistEntityArgument(commerceEntity), context);
            }

            return entityView;
        }

    }
}
