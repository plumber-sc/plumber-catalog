using Plugin.Plumber.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Customers;
using Sitecore.Framework.Pipelines;
using System;
using System.Threading.Tasks;

namespace Plugin.Plumber.Catalog.Pipelines.Blocks.ViewValidators
{
    public class ValidateCustomerViewBlock : PipelineBlock<EntityViewConditionsArgument, EntityViewConditionsArgument, CommercePipelineExecutionContext>
    {
        public override async Task<EntityViewConditionsArgument> Run(EntityViewConditionsArgument arg, CommercePipelineExecutionContext context)
        {
            var viewsPolicy = context.GetPolicy<KnownCustomerViewsPolicy>();

            arg.ValidateEntity(ent => ent is Customer);
            arg.ValidateDisplayView(viewName => viewName.Equals(viewsPolicy.Master, StringComparison.OrdinalIgnoreCase));
            arg.ValidateEditView(action => action.StartsWith("Edit-", StringComparison.OrdinalIgnoreCase));

            return await Task.FromResult(arg);
        }
    }
}
