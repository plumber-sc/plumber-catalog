using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Plugin.Plumber.Catalog.Pipelines.Arguments;
using Sitecore.Framework.Pipelines;

namespace Plugin.Plumber.Catalog.Pipelines
{
    [PipelineDisplayName(Constants.Pipelines.GetEntityViewComponentsPipeline)]
    public class GetEntityViewComponentsPipeline : CommercePipeline<EntityViewComponentsArgument, EntityViewComponentsArgument>, IGetEntityViewComponentsPipeline
    {
        public GetEntityViewComponentsPipeline(IPipelineConfiguration<IGetEntityViewComponentsPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}
