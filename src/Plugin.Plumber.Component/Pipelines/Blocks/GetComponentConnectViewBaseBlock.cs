using Plugin.Plumber.Component.Attributes;
using Plugin.Plumber.Component.Commanders;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Plumber.Component.Pipelines.Blocks
{
    /// <summary>
    ///     Creates a Sitecore data template for all registered components that have the AddTo<i>xxx</i>DataTemplate  attribute
    /// </summary>
    [PipelineDisplayName(Constants.Pipelines.Blocks.GetComponentConnectViewBaseBlock)]
    public abstract class GetComponentConnectViewBaseBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly ComponentViewCommander catalogSchemaCommander;
        private readonly ViewCommander viewCommander;

        public GetComponentConnectViewBaseBlock(ViewCommander viewCommander, ComponentViewCommander catalogSchemaCommander)
        {
            this.viewCommander = viewCommander;
            this.catalogSchemaCommander = catalogSchemaCommander;
        }

        protected abstract bool IsSupportedEntity(CommerceEntity entity);
        protected abstract bool ComponentShouldBeAddedToDataTemplate(System.Attribute[] attrs);
        protected abstract string GetConnectViewName(CommercePipelineExecutionContext context);

        public async override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{Name}: The argument cannot be null.");
            var request = this.viewCommander.CurrentEntityViewArgument(context.CommerceContext);

            // Only proceed if the current entity is a sellable item
            if (!IsSupportedEntity(request.Entity))
            {
                return arg;
            }

            var isConnectView = arg.Name.Equals(GetConnectViewName(context), StringComparison.OrdinalIgnoreCase);            

            // Make sure that we target the correct views
            if (!isConnectView)
            {
                return arg;
            }

            List<Type> allComponentTypes = await catalogSchemaCommander.GetAllComponentTypes(context.CommerceContext);

            var targetView = arg;

            foreach (var componentType in allComponentTypes)
            {
                System.Attribute[] attrs = System.Attribute.GetCustomAttributes(componentType);

                if (attrs.SingleOrDefault(attr => attr is EntityViewAttribute) is EntityViewAttribute entityViewAttribute && 
                    ComponentShouldBeAddedToDataTemplate(attrs)) 
                {
                    // Create a new view and add it to the current entity view.
                    var view = new EntityView
                    {
                        Name = componentType.FullName.Replace(".", "_"),
                        DisplayName = entityViewAttribute?.ViewName ?? componentType.Name,
                        EntityId = arg.EntityId,
                        EntityVersion = arg.EntityVersion                       
                    };

                    arg.ChildViews.Add(view);

                    targetView = view;

                    var props = componentType.GetProperties();

                    foreach (var prop in props)
                    {
                        System.Attribute[] propAttributes = System.Attribute.GetCustomAttributes(prop);

                        if (propAttributes.SingleOrDefault(attr => attr is PropertyAttribute) is PropertyAttribute propAttr)
                        {
                            targetView.Properties.Add(new ViewProperty
                            {
                                Name = prop.Name,
                                DisplayName = propAttr.DisplayName,
                                OriginalType =prop.PropertyType.FullName,
                                IsReadOnly = propAttr.IsReadOnly,
                                IsRequired = propAttr.IsRequired
                            });
                        }
                    }
                }

            }

            return arg;
        }

    }
}
