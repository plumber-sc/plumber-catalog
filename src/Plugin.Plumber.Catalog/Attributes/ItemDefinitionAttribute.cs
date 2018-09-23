using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Plumber.Catalog.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
    public class ItemDefinitionAttribute : System.Attribute
    {
        public string ItemDefinition { get; private set; }

        public ItemDefinitionAttribute(string itemDefinition) => ItemDefinition = itemDefinition;
    }
}
