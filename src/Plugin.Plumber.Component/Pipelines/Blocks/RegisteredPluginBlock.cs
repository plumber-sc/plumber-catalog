using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Plumber.Component.Pipelines.Blocks
{
    public class RegisteredPluginBlock : PipelineBlock<IEnumerable<RegisteredPluginModel>, IEnumerable<RegisteredPluginModel>, CommercePipelineExecutionContext>
    {
        public override Task<IEnumerable<RegisteredPluginModel>> Run(IEnumerable<RegisteredPluginModel> arg, CommercePipelineExecutionContext context)
        {
            if (arg == null)
                return Task.FromResult<IEnumerable<RegisteredPluginModel>>(arg);

            List<RegisteredPluginModel> list = arg.ToList<RegisteredPluginModel>();
            PluginHelper.RegisterPlugin(this, list);

            return Task.FromResult<IEnumerable<RegisteredPluginModel>>(list.AsEnumerable<RegisteredPluginModel>());
        }

        public RegisteredPluginBlock() : base((string)null)
        {
        }
    }
}
