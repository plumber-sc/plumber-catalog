using Sitecore.Commerce.Core;
using Plugin.Plumber.Component.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Plugin.Plumber.Component.Sample.Components
{
    [EntityView(viewName:"Warranty Information",addToCatalogDataTemplate:true)]
    [AllSellableItems]
    public class WarrantyComponent : Sitecore.Commerce.Core.Component
    {
        [Property("Warranty length (months)", showInList:true)]
        [Range(12, 24)]
        public long WarrantyLengthInMonths { get; set; }

        [Property("Additional warranty information", showInList:true)]
        [RegularExpression(pattern: "^(Days|Months|Years)$",
            ErrorMessage ="Valid values are: Days, Months, Years")]
        public string WarrantyInformation { get; set; }
    }
}
