using Plugin.Plumber.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Plumber.Catalog.Pipelines.Blocks.ViewValidators
{
    public class ValidateSellableItemViewBlock : PipelineBlock<EntityViewConditionsArgument, EntityViewConditionsArgument, CommercePipelineExecutionContext>
    {
        public override async Task<EntityViewConditionsArgument> Run(EntityViewConditionsArgument arg, CommercePipelineExecutionContext context)
        {
            var catalogViewsPolicy = context.GetPolicy<KnownCatalogViewsPolicy>();

            arg.ValidateEntity(ent => ent is SellableItem);
            arg.ValidateDisplayView(viewName => viewName.Equals(catalogViewsPolicy.Master, StringComparison.OrdinalIgnoreCase));
            arg.ValidateEditView(viewName => viewName.Equals(catalogViewsPolicy.Variant, StringComparison.OrdinalIgnoreCase));

            return await Task.FromResult(arg);
        }
    }
}
