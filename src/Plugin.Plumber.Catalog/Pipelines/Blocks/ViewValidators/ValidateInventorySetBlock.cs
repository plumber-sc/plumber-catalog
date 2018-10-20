using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Plumber.Catalog.Pipelines.Blocks.ViewValidators
{
    public class ValidateInventorySetBlock : ValidateEntityViewBaseBlock<InventorySet>
    {
        protected override string GetMasterViewName(CommercePipelineExecutionContext context)
        {
            var viewsPolicy = context.GetPolicy<KnownInventoryViewsPolicy>();
            return viewsPolicy?.Master;
        }
    }
}
