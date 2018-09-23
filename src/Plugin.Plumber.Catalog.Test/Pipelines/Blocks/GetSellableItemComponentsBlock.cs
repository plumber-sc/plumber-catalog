using Sitecore.Commerce.Core;
using Plugin.Plumber.Catalog.Pipelines.Arguments;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;
using Plugin.Plumber.Catalog.Sample.Components;

namespace Plugin.Plumber.Catalog.Sample.Pipelines.Blocks
{
    public class GetSellableItemComponentsBlock : PipelineBlock<SellableItemComponentsArgument, SellableItemComponentsArgument, CommercePipelineExecutionContext>
    {
        public async override Task<SellableItemComponentsArgument> Run(SellableItemComponentsArgument arg, CommercePipelineExecutionContext context)
        {
            arg.RegisterComponent<NotesComponent>();
            arg.RegisterComponent<SampleComponent>();
            arg.RegisterComponent<WarrantyComponent>();

            return await Task.FromResult<SellableItemComponentsArgument>(arg);
        }
    }
}
