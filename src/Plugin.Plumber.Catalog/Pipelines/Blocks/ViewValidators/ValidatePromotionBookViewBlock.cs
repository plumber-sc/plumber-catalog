using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Promotions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Plumber.Catalog.Pipelines.Blocks.ViewValidators
{
    public class ValidatePromotionBookViewBlock : ValidateEntityViewBaseBlock<PromotionBook>
    {
        protected override string GetMasterViewName(CommercePipelineExecutionContext context)
        {
            var viewsPolicy = context.GetPolicy<KnownPromotionsViewsPolicy>();
            return viewsPolicy?.Master;
        }
    }
}
