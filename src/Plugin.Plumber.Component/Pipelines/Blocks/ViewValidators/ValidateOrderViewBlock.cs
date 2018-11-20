using Plugin.Plumber.Component.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Orders;
using Sitecore.Commerce.Plugin.Promotions;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Plumber.Component.Pipelines.Blocks.ViewValidators
{
    public class ValidateOrderViewBlock : ValidateEntityViewBaseBlock<Order>
    {
        protected override string GetMasterViewName(CommercePipelineExecutionContext context)
        {
            var orderViewsPolicy = context.GetPolicy<KnownOrderViewsPolicy>();
            return orderViewsPolicy?.Master;
        }
    }
}
