using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Plumber.Catalog.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AddToEntityTypeAttribute : Attribute
    {
        public Type EntityType { get; set; }

        public AddToEntityTypeAttribute(Type entityType)
        {
            this.EntityType = entityType;
        }
    }
}
