using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Plumber.Catalog.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class EntityViewAttribute : System.Attribute
    {
        public string ViewName { get; private set; }
     
        public EntityViewAttribute(string viewName)
        {
            ViewName = viewName;
        }
    }
}
