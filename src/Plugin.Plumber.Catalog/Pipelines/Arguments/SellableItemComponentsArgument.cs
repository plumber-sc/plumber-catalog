using Sitecore.Commerce.Core;
using System;
using System.Collections.Generic;

namespace Plugin.Plumber.Catalog.Pipelines.Arguments
{
    public class SellableItemComponentsArgument : PipelineArgument
    {
        public List<Type> SellableItemComponents { get; private set; }

        public SellableItemComponentsArgument()
        {
            SellableItemComponents = new List<Type>();
        }

        public void RegisterComponent<Type>() where Type : Component
        {
            SellableItemComponents.Add(typeof(Type));
        }
    }
}
