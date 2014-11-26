#Vault for Umbraco 6

**Version 0.9, Designed for Umbraco 6.x**

***Note**: We're already hard at work developing and testing a more refined version of Vault designed to support Umbraco 7. Some features identified in this document may not apply to newer versions of Umbraco Vault. But don't worry, the good stuff will stick around.*

***Also note**: Vault has a lot of features. We're actively working on documenting all of them, but this may take some time; this document is not yet complete.*

TODO: Mention uComponents requirement here? OR: rip out ucomponents requirement because it's dumb as a dependency? 

##Overview
Vault for Umbraco is an easy-to-use, extensible ORM to quickly and easily get strongly-typed Umbraco CMS data into your markup.  It allows you to create lightly-decorated classes that Vault will understand how to hydrate. This gives you the full view model-style experience in Umbraco that you are accustomed to in MVC, complete with strongly-typed view properties (no more magic strings in your views).




##Nuget Installation

TODO: fill in actual nuget package

```
PM> Install-Package Nerdery.Vault.Umbraco.6
```



##Basic Usage Example


###Create a View Model

For our example, let's assume we have a document type with the alias `BlogEntry` set up with the following properties:

Property Name | Alias | Type
--- | --- | ---
Title | title | Textstring
PostedDate | postedDate | Date Picker
Content | content | Richtext editor

We can create a class for this document type:

```
[UmbracoEntity(AutoMap = true)]
public class BlogEntryViewModel
{
	public string Title { get; set; }
	public DateTime PostDate { get; set; }

	[UmbracoRichTextProperty]
	public string Content { get; set; }	
}
```

####What are those attributes?

`UmbracoEntity` tells Vault that it can create an instance of this class. Setting `AutoMap` to `true` instructs Vault to attempt to fill all properties that have public setters unless they are explicitly ignored using the `UmbracoIgnoreProperty` attribute.

The `UmbracoRichTextProperty` attribute is a special case that instructs Vault to process the property as rich text content, which includes the processing of macros.


###Get the content

You can get hydrated instances of the model into your view several different ways. Here are a few basic examples of explicitly getting hydrated models:

```
// Gets a BlogEntry hydrated from the current Node Id
var model = Vault.Context.GetCurrent<BlogEntryViewModel>();
```

```
// Gets a BlogEntry hydrated from a specific Node Id
var model = Vault.Context.GetContentById<BlogEntryViewModel>(myNodeId);
```

So how does it work? By default, Vault will look at each property type, lower-case the first character and use the result as a property alias for which to get data from Umbraco (i.e. the `Title` class property will be given the value of the `title` cms property). 

Like most conventions in Vault, there are ways to override the default behavior.  More on that later.


###Pass it to the view

Once your have your hydrated instance, simply pass it into your view the normal (MVC) way:

```
return View(model);
```

Of course, this setup requires that we create a controller for every template, even simple ones. That's not helping us at all. Let's fix that below.


##Using the Vault Default Controller

Vault ships with two default controllers that extend/replace the Umbraco default controller. These controllers make it incredibly fast and easy to wire up strongly-typed view models to views.

Here's how to get started.

###Default Controller Setup

In your `OnApplicationStarting` event (often found in a class extending UmbracoApplication), place the following code:

```
DefaultRenderMvcControllerResolver
	.Current
	.SetDefaultControllerType(
		typeof(VaultDefaultGenericController));
```

###Create a class

Let's create a view model (just like above). Note the namespace this time.

```
namespace MySite.Models.ContentModels 
{
	[UmbracoEntity(AutoMap = true)]
	public class BlogEntryViewModel
	{
		public string Title { get; set; }
		public DateTime PostDate { get; set; }
	
		[UmbracoRichTextProperty]
		public string Content { get; set; }	
	}
}
```
**An important note about naming conventions:** the Vault generic controller will take the name of the current *template*, append the text "ViewModel" and try to create that type using the currently-registered namespaces (see next step).  This means that when using the Vault default controllers, it is important that the name of the View Model matches the Template Alias, not the Document Type Alias. 

This is less important when your Document Types and Templates having matching aliases, but of course this isn't always the case.

###Register a namespace

Now, let's register the namespace and assembly with Vault *(Note: in the upcoming version of Vault, this step will likely be removed)*.

```
Vault.RegisterViewModelNamespace(
	"MySite.Models.ContentModels", 
	"MySiteAssembly");
```

###Create the view

Finally, we can create our template, called `BlogEntry` in Umbraco:

```
@model MySite.Models.ContentModels.BlogEntryPage
@{
    Layout = "MainLayout.cshtml";
}

	<h1>@Model.Title</h1>
	<div>@Model.PostedDate.ToShortDateString()</div>
	<div>
    	@Html.Raw(@Model.Content)
	</div>

```

###An important note

By default, your view probably had something like this at the top:

```
@inherits Umbraco.Web.Mvc.UmbracoTemplatePage
```

This is necessary when using Umbraco's default controller, but it is not necessary (or correct) when using the `VaultDefaultGenericController` (although it *is* necessary when using the `VaultDefaultRenderController`).

**You will get a runtime error** if you leave the `@inherits ... UmbracoTemplatePage` code at the top of any view in the render chain (including your parent templates).  Simply remove it and be happy you no longer have to deal with it.

###Hold on, what if I need Umbraco node information in my View Model?

There are a variety of reasons one might need Umbraco node information available. The fastest way to get this information in a Vault-hydrated view model is to add a property like this:

```
public IPublishedContent CmsContent { get; set; }
```
The name of the property is not important, but the type is.  *Note that if you have more than one IPublishedContent property in a class, only the first one will get set.*

This property will get set to the node data *that is hydrating the model,* not necessarily the current node's data.  This is especially important when Vault fills out deeper object graphs and/or lists.

Speaking of more advanced modeling scenarios...

##More Advanced Modeling Scenarios

The real power of Vault starts to become apparent when you consider how easily it allows you to handle a variety of modeling scenarios in a variety of ways. Let's investigate some of these.

###Nested View Models

Consider the following scenario: let's say we have a HomePage doc type (and associated template) that, among other things, allows the content editor to select a featured blog post using a simple Content Picker. Let's say the content picker has the alias `featuredBlogEntry`.

Here's how we might want our view model to look:

```
[UmbracoEntity(AutoMap = true)]
public class HomePageViewModel
{
	public string Title { get; set; }
	public string SidebarCopy { get; set; }
	public BlogEntryViewModel FeaturedBlogEntry { get; set; }
}
```
In this case, Vault will create the HomePageViewModel instance as you would expect, fully hydrating both the `HomePageViewModel` basic properties as well as all the properties in the `FeaturedBlogEntry` property.

For a deeper understanding of how Vault determines how to hydrate properties, including nested types, see the section on Type Handlers below.

***A word of caution:** Vault currently has no depth limits for hydrating object graphs. This comes up less often with view models, but keep in mind that deep graphs or recursion could cause performance problems and/or infinite loops.  Future versions of Vault may include limits and opt-outs for such situations.*


###Collections and Lists

TODO: keep it rolling


##Handling media

TODO: describe

##Breaking Conventions

Many of Vault's conventions can be circumvented as needed.  

TODO: List the ways


##Meet the Context Methods

TODO: list and describe the context methods

##Extending Vault

TODO: the deep stuff, like TypeHandlers, etc

##uComponenets Integration

TODO: yep, the drill