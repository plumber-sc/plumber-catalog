using Sitecore.Commerce.Core;
using Plugin.Plumber.Component.Pipelines.Arguments;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;
using Plugin.Plumber.Component.Sample.Components;

namespace Plugin.Plumber.Component.Sample.Pipelines.Blocks
{
    public class GetEntityViewComponentsBlock : PipelineBlock<EntityViewComponentsArgument, EntityViewComponentsArgument, CommercePipelineExecutionContext>
    {
        public async override Task<EntityViewComponentsArgument> Run(EntityViewComponentsArgument arg, CommercePipelineExecutionContext context)
        {
            arg.RegisterComponent<NotesComponent>();
            arg.RegisterComponent<SampleComponent>();
            arg.RegisterComponent<WarrantyComponent>();

            return await Task.FromResult<EntityViewComponentsArgument>(arg);
        }
    }
}
