using Plugin.Plumber.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Orders;
using Sitecore.Commerce.Plugin.Promotions;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Plumber.Catalog.Pipelines.Blocks.ViewValidators
{
    public class ValidateOrderViewBlock : PipelineBlock<EntityViewConditionsArgument, EntityViewConditionsArgument, CommercePipelineExecutionContext>
    {
        public override async Task<EntityViewConditionsArgument> Run(EntityViewConditionsArgument arg, CommercePipelineExecutionContext context)
        {
            var orderViewsPolicy = context.GetPolicy<KnownOrderViewsPolicy>();

            arg.ValidateEntity(ent => ent is Order);
            arg.ValidateDisplayView(viewName => viewName.Equals(orderViewsPolicy.Master, StringComparison.OrdinalIgnoreCase));
            arg.ValidateEditView(action => action.StartsWith("Edit-", StringComparison.OrdinalIgnoreCase));

            return await Task.FromResult(arg);
        }
    }
}
