using Plugin.Plumber.Component.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Plumber.Component.Pipelines.Blocks.ViewValidators
{
    public class ValidateSellableItemViewBlock : ValidateEntityViewBaseBlock<SellableItem>
    {/*
        public override async Task<EntityViewConditionsArgument> Run(EntityViewConditionsArgument arg, CommercePipelineExecutionContext context)
        {
            arg.ValidateEntity(ent => ent is SellableItem);
            arg.ValidateDisplayView(viewName => viewName.Equals(catalogViewsPolicy.Master, StringComparison.OrdinalIgnoreCase));
            arg.ValidateEditView(viewName => viewName.Equals(catalogViewsPolicy.Variant, StringComparison.OrdinalIgnoreCase));

            return await Task.FromResult(arg);
        }
        */
        protected override string GetMasterViewName(CommercePipelineExecutionContext context)
        {
            var catalogViewsPolicy = context.GetPolicy<KnownCatalogViewsPolicy>();
            return catalogViewsPolicy?.Master;
        }
    }
}
