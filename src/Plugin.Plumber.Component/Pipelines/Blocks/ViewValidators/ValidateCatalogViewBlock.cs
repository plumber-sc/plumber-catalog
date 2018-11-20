using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;

namespace Plugin.Plumber.Component.Pipelines.Blocks.ViewValidators
{
    public class ValidateCatalogViewBlock : ValidateEntityViewBaseBlock<Sitecore.Commerce.Plugin.Catalog.Catalog>
    {
        protected override string GetMasterViewName(CommercePipelineExecutionContext context)
        {
            var viewsPolicy = context.GetPolicy<KnownCatalogViewsPolicy>();
            return viewsPolicy?.Master;
        }
    }
}
