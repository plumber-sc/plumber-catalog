using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Plugin.Plumber.Component.Pipelines;
using Plugin.Plumber.Component.Pipelines.Blocks;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines.Definitions.Extensions;
using Plugin.Plumber.Component.Pipelines.Blocks.ViewValidators;

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
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);
            services.RegisterAllCommands(assembly);

            services.Sitecore().Pipelines(
                config =>
                    config
                        .ConfigurePipeline<IGetEntityViewPipeline>(c =>
                        {
                            c
                            .Add<GetComponentViewBlock>().After<GetSellableItemDetailsViewBlock>()
                            .Add<GetCatalogComponentConnectViewBlock>().After<GetComponentViewBlock>()
                            .Add<GetCategoryComponentConnectViewBlock>().After<GetCatalogComponentConnectViewBlock>()
                            .Add<GetSellableItemComponentConnectViewBlock>().After<GetCategoryComponentConnectViewBlock>();
                        })
                        .ConfigurePipeline<IPopulateEntityViewActionsPipeline>(c =>
                        {
                            c.Add<PopulateComponentActionsBlock>().After<InitializeEntityViewActionsBlock>();
                        })
                        .ConfigurePipeline<IDoActionPipeline>(c =>
                        {
                            c.Add<DoActionEditComponentBlock>().After<ValidateEntityVersionBlock>()
                            .Add<DoActionAddValidationConstraintBlock>().Before<DoActionEditComponentBlock>();
                        })
                        .AddPipeline<IGetEntityViewComponentsPipeline, GetEntityViewComponentsPipeline>()
                        .AddPipeline<IGetApplicableViewConditionsPipeline, GetApplicableViewConditionsPipeline>( c=> 
                        {
                            c.Add<ValidateSellableItemViewBlock>()
                            .Add<ValidatePromotionViewBlock>()
                            .Add<ValidateOrderViewBlock>()
                            .Add<ValidateCustomerViewBlock>()
                            .Add<ValidateInventorySetBlock>()
                            .Add<ValidateCatalogViewBlock>()
                            .Add<ValidatePromotionBookViewBlock>()
                            .Add<ValidatePriceBookViewBlock>()
                            .Add<ValidatePriceCardViewBlock>()
                            .Add<ValidateCategoryViewBlock>();
                        })
                        .ConfigurePipeline<IConfigureServiceApiPipeline>(c => c.Add<ConfigureServiceApiBlock>()));

        }
    }
}