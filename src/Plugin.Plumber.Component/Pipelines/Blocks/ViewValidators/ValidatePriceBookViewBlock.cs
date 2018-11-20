using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Pricing;

namespace Plugin.Plumber.Component.Pipelines.Blocks.ViewValidators
{
    public class ValidatePriceBookViewBlock : ValidateEntityViewBaseBlock<PriceBook>
    {
        protected override string GetMasterViewName(CommercePipelineExecutionContext context)
        {
            var viewsPolicy = context.GetPolicy<KnownPricingViewsPolicy>();
            return viewsPolicy?.Master;
        }
    }
}
