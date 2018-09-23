[![Build status](https://ci.appveyor.com/api/projects/status/s5nq8u1fo4klxt0x?svg=true)](https://ci.appveyor.com/project/ewerkman/plumber-catalog)

# Plumber Catalog for Sitecore Commerce

> Plugin for Sitecore Commerce that allows you to add attributes to your catalog components for easy addition to the Sitecore Commerce Business tools.

## What is this?

Since Sitecore Commerce 9.0 update 2, you can extend a Sitecore Commerce Catalog in two ways:

* Use the Entity Composer to create templates that add new properties to your catalog;
* Create your own components that add properties as POCO classes and write pipeline blocks that add functionality to the Business tools to edit these properties;

The first way is easy to do but has the disadvantage that there is no (easy) typesafe way to work with these properties in code. 

The second way gives you type safety but it makes you write a lot of plumbing to support viewing and editing these properties in the business tools.

**Plumber Catalog** is a plugin for Sitecore Commerce that gives you the ability to add attributes to your catalog component classes that contain meta information about the component.  
It adds pipeline blocks to the IGetEntityViewPipeline (which is responsible for providing the views for the Business Tools) that take the meta information of your component and adds the appropriate views and actions to the business tools. 

This means you just add attributes to your catalog component class and Plumber Catalog will take care of the rest.

## How to use it? 

Add a dependency on Plumber.Catalog to the plugin that contains your catalog components:

* From the package manager console: `Install-Package Plugin.Plumber.Catalog` 
* Using the Nuget package manager add a dependency on `Plugin.Plumber.Catalog`.

## Getting started

Let's say you want to extend a sellable item with information on the warranty. You want to add two fields: 

* An integer value indicating the number of months warranty you get with the product;
* A piece of text (string) giving more information on the warranty.

This is what your component would look like:


```c#
using Sitecore.Commerce.Core;

namespace Plugin.Plumber.Catalog.Sample.Components
{
	public class WarrantyComponent : Component
	{
		public int WarrantyLengthInMonths { get; set; }

		public string WarrantyInformation { get; set; }
	}
}
```

Now, if you want users to be able to edit the warranty information in the Merchandising Manager you would normally have to:

* Add a block to the `IGetEntityViewPipeline` to create an entity view for the Merchandising Manager
* Add a block to the `IPopulateEntityViewActionsPipeline` to add an action to the entity view so the user can edit the data.
* Add a block to the `IDoActionPipeline` to save the data the user edited.
* Add another block to the `IGetEntityViewPipeline` to handle updating the Sitecore template for a sellable item.

Instead, with __Plumber Catalog__ you do the following:

1. Add a dependency on the Plumber.Catalog Nuget package. You can use the package manager console and execute the following command: `Install-Package Plugin.Plumber.Catalog` or add a dependency on `Plugin.Plumber.Catalog` using the Nuget Package Manager.
2. Add some attributes to the `WarrantyComponent` class so it looks like this:


```c#
using Sitecore.Commerce.Core;
using Plugin.Plumber.Catalog.Attributes;

namespace Plugin.Plumber.Catalog.Sample.Components
{
	[EntityView("Warranty Information")]
	[AllSellableItems]
	public class WarrantyComponent : Component
    {
        [Property("Warranty length (months)"]
        public int WarrantyLengthInMonths { get; set; }

        [Property("Additional warranty information", )]
        public string WarrantyInformation { get; set; }
    }
}
```
This code does three things:

 - The `EntityView` attribute indicates you want to use this component in an entity view in the Merchandising Manager. 
 - The `AllSellableItems` attribute indicates this component should be added to all sellable items. There is also an `ItemDefinition` attribute you can use to add a component based on the item definition of a sellable item.
 - Adds a `Property` attribute to each 

3. Plumber.Catalog needs to know that the `WarrantyComponent` is a component that can be added to a `SellableItem`.  To register your components with Plumber.Catalog, you create a pipeline block and add it to the `IGetSellableItemComponentsPipeline`. Plumber.Catalog runs this pipeline to get a list of all the components that can be added to a sellable item. The pipeline block to register types looks like this:
```c#
using Sitecore.Commerce.Core;
using Plugin.Plumber.Catalog.Pipelines.Arguments;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;
using Plugin.Plumber.Catalog.Sample.Components;

namespace Plugin.Plumber.Catalog.Sample.Pipelines.Blocks
{
    public class GetSellableItemComponentsBlock : PipelineBlock<SellableItemComponentsArgument, SellableItemComponentsArgument, CommercePipelineExecutionContext>
    {
        public async override Task<SellableItemComponentsArgument> Run(SellableItemComponentsArgument arg, CommercePipelineExecutionContext context)
        {
            arg.Register<WarrantyComponent>();

            return await Task.FromResult<SellableItemComponentsArgument>(arg);
        }
    }
}
```
This code registers three components with Plumber.Catalog. 



## Available Attributes

Below you will find the attributes you can add to your components.

### EntityViewAttribute

Add the `EntityVuewAttribute` to a class to indicate the class should be added as an entity view in the BizFx tools.

|Parameter|Description|
|---------|-----------|
|`ViewName`|Name of the view to show in the Merchandising Manager|


### ItemDefinitionAttribute

Add the `ItemDefinitionAttribute` to a component class to specify the item definition name this component should be added to.

|Parameter|Description|
|---------|-----------|
|`ItemDefinitnion`|Name of the item definition for which to add this component to a sellable item|


### PropertyAttribute

Add a `PropertyAttribute` to each property of the class you want to be visible in the entity view in the Merchandising Manager.

|Parameter|Description|
|---------|-----------|
|`DisplayName`|Name of the property shown in the Merchandising Manager|
|`IsReadOnly`|Set to `true` to indicate this property cannot be edited in the Merchandising Manager|
|`IsRequired`|Set to `true` if this property is required.|

## Validation
You can add validation attributes from the `System.ComponentModel.DataAnnotations` namespace to the properties of your components so they can be automatically be validated in the Merchandising Manager. 

Plumber.Catalog adds the `DoActionAddValidationConstraintsBlock` to the `IDoActionPipeline` pipeline, which will validate the entered data and adds error messages if necessary.

Below is an example of using the data annotations attributes:

```c#
using Sitecore.Commerce.Core;
using Plugin.Plumber.Catalog.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Plugin.Plumber.Catalog.Sample.Components
{
    [EntityView("Warranty Information")]
    [AllSellableItems]
    public class WarrantyComponent : Component
    {
        [Property("Warranty length (months)", showInList:true)]
        [Range(12, 24)]
        public int WarrantyLengthInMonths { get; set; }

        [Property("Additional warranty information", showInList:true)]
        [RegularExpression(pattern: "^(Days|Months|Years)$",
            ErrorMessage ="Valid values are: Days, Months, Years")]
        public string WarrantyInformation { get; set; }
    }
}
```

In this example: 

* the`WarrantyLengthInMonths` property has to be in the range 12 to 24;
* the `WarrantyInformation` property is validated by a `RegularExpression` validator and has to be one of `Days`, `Months` or `Years`,