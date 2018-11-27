using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Plumber.Component.Attributes.SellableItem
{
    

    [System.AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AddToSellableItemAttribute : SellableItemAttributeBase
    {
        public AddToSellableItemAttribute(AddToSellableItem addToSellableItem) : base(addToSellableItem)
        {
        }
    }
}
