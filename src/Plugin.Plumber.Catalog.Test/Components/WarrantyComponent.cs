using Sitecore.Commerce.Core;
using Plugin.Plumber.Catalog.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Plugin.Plumber.Catalog.Sample.Components
{
    [EntityView("Warranty Information")]
    [AllSellableItems]
    public class WarrantyComponent : Component
    {
        [Property("Warranty length (months)", showInList:true)]
        [Range(12, 24)]
        public int WarrantyLengthInMonths { get; set; }

        [Property("Additional warranty information", showInList:true)]
        [RegularExpression(pattern: "^(Days|Months|Years)$",
            ErrorMessage ="Valid values are: Days, Months, Years")]
        public string WarrantyInformation { get; set; }
    }
}
