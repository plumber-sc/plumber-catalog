using Plugin.Plumber.Component.Attributes;
using Plugin.Plumber.Component.Commanders;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Plumber.Component.Pipelines.Blocks
{
    /// <summary>
    ///     Validates the data entered against the validation attributes. 
    /// </summary>
    [PipelineDisplayName(Constants.Pipelines.Blocks.DoActionAddValidationConstraintBlock)]
    public class DoActionAddValidationConstraintBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly CatalogSchemaCommander catalogSchemaCommander;

        public DoActionAddValidationConstraintBlock(CatalogSchemaCommander catalogSchemaCommander)
        {
            this.catalogSchemaCommander = catalogSchemaCommander;
        }

        public override async Task<EntityView> Run(EntityView entityView, CommercePipelineExecutionContext context)
        {
            Condition.Requires(entityView).IsNotNull($"{Name}: The argument cannot be null.");

            // Only proceed if the right action was invoked
            if (string.IsNullOrEmpty(entityView.Action) || !entityView.Action.StartsWith("Edit-", StringComparison.OrdinalIgnoreCase))
            {
                return entityView;
            }

            // Check if we have a sellable item on the context
            var commerceEntity = context.CommerceContext.GetObject<CommerceEntity>(x => x.Id.Equals(entityView.EntityId));
            if (commerceEntity == null)
            {
                return entityView;
            }
            
            var applicableComponentTypes = await this.catalogSchemaCommander.GetApplicableComponentTypes(context.CommerceContext, commerceEntity);
            var editedComponentType = applicableComponentTypes.SingleOrDefault(comp => entityView.Action == $"Edit-{comp.FullName}");

            if (editedComponentType != null)
            {
                // Get the component from the sellable item or its variation
                var editedComponent = catalogSchemaCommander.GetEditedComponent(commerceEntity, editedComponentType);
              
                await ValidateConstraints(entityView.Properties,
                    editedComponentType, editedComponent, context);
            }

            return entityView;
        }

        private async Task<bool> ValidateConstraints(List<ViewProperty> properties,
                        Type editedComponentType,
                        Sitecore.Commerce.Core.Component editedComponent,
                        CommercePipelineExecutionContext context)
        {
            var result = true;
            var props = editedComponentType.GetProperties();

            foreach (var prop in props)
            {
                var fieldValueAsString = properties.FirstOrDefault(x => x.Name.Equals(prop.Name, StringComparison.OrdinalIgnoreCase))?.Value;

                Attribute[] propAttributes = Attribute.GetCustomAttributes(prop);

                var propertyAttribute = propAttributes.SingleOrDefault(attr => attr is PropertyAttribute) as PropertyAttribute;

                if (propertyAttribute != null)
                {
                    var isValidProperty = await ValidateProperty(prop, fieldValueAsString, propAttributes, propertyAttribute, context);
                    if(!isValidProperty)
                    {
                        result = false;
                    }
                }
            }

            return result;
        }

        private async Task<bool> ValidateProperty(System.Reflection.PropertyInfo prop, string fieldValueAsString, Attribute[] propAttributes, PropertyAttribute propertyAttribute, CommercePipelineExecutionContext context)
        {
            var validationAttributes = propAttributes.OfType<ValidationAttribute>();
            var validationResults = new List<ValidationResult>();

            var valid = Validator.TryValidateValue(fieldValueAsString, new ValidationContext(fieldValueAsString, null, null), validationResults, validationAttributes);
            if (!valid)
            {
                foreach (var validationResult in validationResults)
                {
                    KnownResultCodes errorCodes = context.CommerceContext.GetPolicy<KnownResultCodes>();
                    var str = await context.CommerceContext.AddMessage(errorCodes.ValidationError, "InvalidPropertyValueRange", new object[1]
                          {
                                propertyAttribute?.DisplayName ?? prop.Name
                          }, $"There was an error for '{ propertyAttribute?.DisplayName ?? prop.Name }': '{validationResult.ErrorMessage}'.");
                }
            }

            return valid;
        }
    }
}
