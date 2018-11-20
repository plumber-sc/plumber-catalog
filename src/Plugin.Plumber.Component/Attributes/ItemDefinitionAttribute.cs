using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Plumber.Component.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
    public class AddToItemDefinitionAttribute : System.Attribute
    {
        public string ItemDefinition { get; private set; }

        public AddToItemDefinitionAttribute(string itemDefinition) => ItemDefinition = itemDefinition;
    }
}
