using Plugin.Plumber.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Plumber.Catalog.Pipelines
{
    public interface IGetApplicableViewConditionsPipeline : IPipeline<EntityViewConditionsArgument, EntityViewConditionsArgument, CommercePipelineExecutionContext>
    {
    }
}
