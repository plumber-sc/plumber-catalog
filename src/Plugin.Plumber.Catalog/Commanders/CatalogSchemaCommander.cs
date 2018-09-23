using Microsoft.Extensions.Logging;
using Plugin.Plumber.Catalog.Attributes;
using Plugin.Plumber.Catalog.Pipelines;
using Plugin.Plumber.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Plumber.Catalog.Commanders
{
    /// <summary>
    ///     Helper class 
    /// </summary>
    public class CatalogSchemaCommander : CommerceCommander
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public CatalogSchemaCommander(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        /// <summary>
        ///     Returns all registered component types  
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<List<Type>> GetAllComponentTypes(CommerceContext context)
        {
            var sellableItemComponentsArgument = new SellableItemComponentsArgument();
            sellableItemComponentsArgument = await this.Pipeline<IGetSellableItemComponentsPipeline>().Run(sellableItemComponentsArgument, context.GetPipelineContext());

            return sellableItemComponentsArgument.SellableItemComponents;
        }

        /// <summary>
        ///     Retrieves all component types applicable for the sellable item
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sellableItem">Sellable item for which to get the applicable components</param>
        /// <returns></returns>
        public async Task<List<Type>> GetApplicableComponentTypes(CommerceContext context, SellableItem sellableItem)
        {
            // Get the item definition
            var catalogs = sellableItem.GetComponent<CatalogsComponent>();

            // TODO: What happens if a sellableitem is part of multiple catalogs?
            var catalog = catalogs.GetComponent<CatalogComponent>();
            var itemDefinition = catalog.ItemDefinition;

            var sellableItemComponentsArgument = new SellableItemComponentsArgument();
            sellableItemComponentsArgument = await this.Pipeline<IGetSellableItemComponentsPipeline>().Run(sellableItemComponentsArgument, context.GetPipelineContext());

            var applicableComponentTypes = new List<Type>();
            foreach (var component in sellableItemComponentsArgument.SellableItemComponents)
            {
                System.Attribute[] attrs = System.Attribute.GetCustomAttributes(component);

                if (attrs.Any(attr => attr is AllSellableItemsAttribute))
                {
                    applicableComponentTypes.Add(component);
                }
                else if (attrs.Any(attr => attr is ItemDefinitionAttribute && ((ItemDefinitionAttribute)attr).ItemDefinition == itemDefinition))
                {
                    applicableComponentTypes.Add(component);
                }

            }

            return applicableComponentTypes;
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="sellableItem"></param>
        /// <param name="editedComponentType"></param>
        /// <returns></returns>
        public Sitecore.Commerce.Core.Component GetEditedComponent(SellableItem sellableItem, Type editedComponentType)
        {
            Sitecore.Commerce.Core.Component component = sellableItem.Components.SingleOrDefault(comp => comp.GetType() == editedComponentType);
            if (component == null)
            {
                component = (Sitecore.Commerce.Core.Component)Activator.CreateInstance(editedComponentType);
                sellableItem.Components.Add(component);
            }

            return component;
        }

        public void SetPropertyValuesOnEditedComponent(List<ViewProperty> properties,
            Type editedComponentType,
            Sitecore.Commerce.Core.Component editedComponent,
            CommerceContext context)
        {
            // Map entity view properties to component
            var props = editedComponentType.GetProperties();

            foreach (var prop in props)
            {
                System.Attribute[] propAttributes = System.Attribute.GetCustomAttributes(prop);

                if (propAttributes.SingleOrDefault(attr => attr is PropertyAttribute) is PropertyAttribute propAttr)
                {
                    var fieldValue = properties.FirstOrDefault(x => x.Name.Equals(prop.Name, StringComparison.OrdinalIgnoreCase))?.Value;

                    TypeConverter converter = TypeDescriptor.GetConverter(prop.PropertyType);
                    if (converter.CanConvertFrom(typeof(string)) && converter.CanConvertTo(prop.PropertyType))
                    {
                        try
                        {
                            object propValue = converter.ConvertFromString(fieldValue);
                            prop.SetValue(editedComponent, propValue);
                        }
                        catch (Exception)
                        {
                            context.Logger.LogError($"Could not convert property '{prop.Name}' with value '{fieldValue}' to type '{prop.PropertyType}'");
                        }
                    }
                }
            }
        }

    }
}
