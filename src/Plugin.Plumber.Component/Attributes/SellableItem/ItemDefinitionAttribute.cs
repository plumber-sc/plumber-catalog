using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Plumber.Component.Attributes.SellableItem
{
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
    public class AddToItemDefinitionAttribute : SellableItemAttributeBase
    {
        private string v;

        public string ItemDefinition { get; private set; }

        public AddToItemDefinitionAttribute(string itemDefinition, AddToSellableItem addToSellableItem) : base(addToSellableItem)
        {
            ItemDefinition = itemDefinition;
        }

        public AddToItemDefinitionAttribute(string v)
        {
            this.v = v;
        }
    }
}
