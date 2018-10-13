using Sitecore.Commerce.Core;
using System;
using System.Collections.Generic;

namespace Plugin.Plumber.Catalog.Pipelines.Arguments
{
    public class EntityViewComponentsArgument : PipelineArgument
    {
        public List<Type> Components { get; private set; }

        public EntityViewComponentsArgument()
        {
            Components = new List<Type>();
        }

        public void RegisterComponent<Type>() where Type : Component
        {
            Components.Add(typeof(Type));
        }
    }
}
