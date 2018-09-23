using Sitecore.Commerce.Core;
using Plugin.Plumber.Catalog.Pipelines.Arguments;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Plumber.Catalog.Pipelines
{
    [PipelineDisplayName(Constants.Pipelines.GetSellableItemComponentsPipeline)]
    public interface IGetSellableItemComponentsPipeline : 
        IPipeline<SellableItemComponentsArgument, 
        SellableItemComponentsArgument, 
        CommercePipelineExecutionContext>
    {
    }
}
