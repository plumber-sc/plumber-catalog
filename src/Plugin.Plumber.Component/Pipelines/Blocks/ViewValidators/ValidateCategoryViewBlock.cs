using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Plumber.Component.Pipelines.Blocks.ViewValidators
{
    public class ValidateCategoryViewBlock : ValidateEntityViewBaseBlock<Category>
    {
        protected override string GetMasterViewName(CommercePipelineExecutionContext context)
        {
            var viewsPolicy = context.GetPolicy<KnownCatalogViewsPolicy>();
            return viewsPolicy?.Master;
        }
    }
}
