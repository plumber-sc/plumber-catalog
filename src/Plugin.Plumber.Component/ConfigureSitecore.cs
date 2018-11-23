using Microsoft.Extensions.DependencyInjection;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines.Definitions.Extensions;
using System.Reflection;

namespace Plugin.Plumber.Component
{
    /// <summary>
    /// The configure sitecore class.
    /// </summary>
    public class ConfigureSitecore : IConfigureSitecore
    {
        /// <summary>
        /// The configure services.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        public void ConfigureServices(IServiceCollection services)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);
            services.RegisterAllCommands(assembly);

            services.Sitecore()
                .ConfigureGetEntityViewPipeline()
                .ConfigurePopulateEntityViewActionsPipeline()
                .ConfigureDoActionPipeline()
                .AddGetEntityViewComponentsPipeline()
                .AddGetApplicableViewConditionsPipeline()
                .ConfigureConfigureServiceApiPipeline()
                .Pipelines(config =>
                        config.ConfigurePipeline<IRunningPluginsPipeline>(c =>
                            c.Add<Pipelines.Blocks.RegisteredPluginBlock>())
                        );

        }
    }
}