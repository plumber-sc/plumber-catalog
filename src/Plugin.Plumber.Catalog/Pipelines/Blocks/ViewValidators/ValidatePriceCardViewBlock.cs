using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Plumber.Catalog.Pipelines.Blocks.ViewValidators
{
    public class ValidatePriceCardViewBlock : ValidateEntityViewBaseBlock<PriceCard>
    {
        protected override string GetMasterViewName(CommercePipelineExecutionContext context)
        {
            var viewsPolicy = context.GetPolicy<KnownPricingViewsPolicy>();
            return viewsPolicy?.Master;
        }
    }
}
