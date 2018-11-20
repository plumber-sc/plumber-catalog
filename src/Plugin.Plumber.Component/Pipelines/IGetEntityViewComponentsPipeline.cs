using Sitecore.Commerce.Core;
using Plugin.Plumber.Component.Pipelines.Arguments;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Plumber.Component.Pipelines
{
    [PipelineDisplayName(Constants.Pipelines.GetEntityViewComponentsPipeline)]
    public interface IGetEntityViewComponentsPipeline : 
        IPipeline<EntityViewComponentsArgument, 
        EntityViewComponentsArgument, 
        CommercePipelineExecutionContext>
    {
    }
}
