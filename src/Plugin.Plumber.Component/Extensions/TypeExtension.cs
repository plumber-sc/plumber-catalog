using Plugin.Plumber.Component.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Plumber.Component.Extensions
{
    public static class TypeExtensions
    {
        public static bool HasClassAttribute(this Type type, Type attributeType)
        {
            return type.GetCustomAttributes(false).Any(attr => attr.GetType() == attributeType);
        }

        public static EntityViewAttribute GetEntityViewAttribute(this Type type)
        {
            return type.GetCustomAttributes(false).SingleOrDefault(attr => attr.GetType() == typeof(EntityViewAttribute)) as EntityViewAttribute;
        }
    }
}
