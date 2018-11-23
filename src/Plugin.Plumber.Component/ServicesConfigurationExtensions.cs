using Microsoft.Extensions.DependencyInjection;
using Plugin.Plumber.Component.Pipelines;
using Plugin.Plumber.Component.Pipelines.Blocks;
using Plugin.Plumber.Component.Pipelines.Blocks.ViewValidators;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines.Definitions.Extensions;

namespace Plugin.Plumber.Component
{
    public static class ServicesConfigurationExtensions
    {
        public static ISitecoreServicesConfiguration ConfigureGetEntityViewPipeline(this ISitecoreServicesConfiguration services)
        {
            services.Pipelines(
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
            );
            return services;
        }

        public static ISitecoreServicesConfiguration ConfigurePopulateEntityViewActionsPipeline(this ISitecoreServicesConfiguration services)
        {
            services.Pipelines(
                config =>
                    config
                        .ConfigurePipeline<IPopulateEntityViewActionsPipeline>(c =>
                        {
                            c.Add<PopulateComponentActionsBlock>().After<InitializeEntityViewActionsBlock>();
                        })
            );
            return services;
        }

        public static ISitecoreServicesConfiguration ConfigureDoActionPipeline(this ISitecoreServicesConfiguration services)
        {
            services.Pipelines(
                config =>
                    config
                        .ConfigurePipeline<IDoActionPipeline>(c =>
                        {
                            c.Add<DoActionEditComponentBlock>().After<ValidateEntityVersionBlock>()
                            .Add<DoActionAddValidationConstraintBlock>().Before<DoActionEditComponentBlock>();
                        })
            );
            return services;
        }

        public static ISitecoreServicesConfiguration AddGetEntityViewComponentsPipeline(this ISitecoreServicesConfiguration services)
        {
            services.Pipelines(
                config =>
                    config.AddPipeline<IGetEntityViewComponentsPipeline, GetEntityViewComponentsPipeline>()
            );
            return services;
        }

        public static ISitecoreServicesConfiguration AddGetApplicableViewConditionsPipeline(this ISitecoreServicesConfiguration services)
        {
            services.Pipelines(
                config =>
                    config.
                        AddPipeline<IGetApplicableViewConditionsPipeline, GetApplicableViewConditionsPipeline>(c =>
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
            );
            return services;
        }

        public static ISitecoreServicesConfiguration ConfigureConfigureServiceApiPipeline(this ISitecoreServicesConfiguration services)
        {
            services.Pipelines(
                config =>
                    config.ConfigurePipeline<IConfigureServiceApiPipeline>(c => c.Add<ConfigureServiceApiBlock>())
            );
            return services;
        }
    }
}
