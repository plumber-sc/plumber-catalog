using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Plumber.Component.Attributes
{
    /// <summary>
    /// Add this attribute to your component to indicate it should be added as a view to the Business tools for the entity
    /// </summary>
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
