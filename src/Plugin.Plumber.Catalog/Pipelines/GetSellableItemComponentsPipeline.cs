using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Plugin.Plumber.Catalog.Pipelines.Arguments;
using Sitecore.Framework.Pipelines;

namespace Plugin.Plumber.Catalog.Pipelines
{
    [PipelineDisplayName(Constants.Pipelines.GetSellableItemComponentsPipeline)]
    public class GetSellableItemComponentsPipeline : CommercePipeline<SellableItemComponentsArgument, SellableItemComponentsArgument>, IGetSellableItemComponentsPipeline
    {
        public GetSellableItemComponentsPipeline(IPipelineConfiguration<IGetSellableItemComponentsPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}
