using Plugin.Plumber.Component.Attributes;
using Plugin.Plumber.Component.Commanders;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using System.Linq;

namespace Plugin.Plumber.Component.Pipelines.Blocks
{
    public class GetCatalogComponentConnectViewBlock : GetComponentConnectViewBaseBlock
    {
        public GetCatalogComponentConnectViewBlock(ViewCommander viewCommander, ComponentViewCommander catalogSchemaCommander) : base(viewCommander, catalogSchemaCommander)
        {
        }

        protected override bool ComponentShouldBeAddedToDataTemplate(System.Attribute[] attrs)
        {
            return attrs.SingleOrDefault(attr => attr is AddToAllEntityTypesAttribute) is AddToAllEntityTypesAttribute
                || attrs.SingleOrDefault(attr => attr is AddToEntityTypeAttribute && ((AddToEntityTypeAttribute) attr).EntityType == typeof(Catalog)) != null;
        }

        protected override string GetConnectViewName(CommercePipelineExecutionContext context)
        {
            var catalogViewsPolicy = context.GetPolicy<KnownCatalogViewsPolicy>();
            return catalogViewsPolicy.ConnectCatalog;
        }

        protected override bool IsSupportedEntity(CommerceEntity entity)
        {
            return entity is Catalog;
        }
    }
}
