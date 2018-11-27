using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Plumber.Component.Attributes.SellableItem
{
    public enum AddToSellableItem { SellableItemOnly, VariantOnly, SellableItemAndVariant }

    public class SellableItemAttributeBase : System.Attribute
    {
        public AddToSellableItem AddToSellableItem { get; private set; }

        public SellableItemAttributeBase(AddToSellableItem addToSellableItem = AddToSellableItem.SellableItemAndVariant)
        {
            AddToSellableItem = addToSellableItem;
        }
    }
}
