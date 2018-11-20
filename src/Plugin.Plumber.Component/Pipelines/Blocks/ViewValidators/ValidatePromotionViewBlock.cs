using Plugin.Plumber.Component.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Promotions;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Plumber.Component.Pipelines.Blocks.ViewValidators
{
    public class ValidatePromotionViewBlock : ValidateEntityViewBaseBlock<Promotion>
    {
        protected override string GetMasterViewName(CommercePipelineExecutionContext context)
        {
            var promotionsViewsPolicy = context.GetPolicy<KnownPromotionsViewsPolicy>();
            return promotionsViewsPolicy.Master;
        }
    }
}
