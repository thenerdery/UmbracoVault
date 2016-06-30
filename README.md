#Vault for Umbraco 6 and 7

[![NuGet](https://img.shields.io/nuget/v/UmbracoVault.svg?style=flat-square)](https://www.nuget.org/packages/UmbracoVault)
[![Build status](https://img.shields.io/appveyor/ci/TheNerdery/umbracovault.svg?style=flat-square)](https://ci.appveyor.com/project/TheNerdery/umbracovault)
[![License](https://img.shields.io/github/license/thenerdery/UmbracoVault.svg?style=flat-square)](https://github.com/thenerdery/UmbracoVault/blob/master/LICENSE)

## Overview
Vault for Umbraco is an easy-to-use, extensible ORM to quickly and easily get strongly-typed Umbraco CMS data into your
 markup.  It allows you to create lightly-decorated classes that Vault will understand how to hydrate. This gives you
 the full view model-style experience in Umbraco that you are accustomed to in MVC, complete with strongly-typed view
 properties (no more magic strings in your views).

## Crash Course

Let's assume we have a document type with the alias `BlogEntry` set up with the following properties:

Property Name | Alias | Umbraco Property Editor Type
--- | --- | ---
Title | title | Textstring
PostedDate | postedDate | Date Picker
Content | content | Richtext editor

We can create a class for this document type:

```csharp
[UmbracoEntity(AutoMap = true)]
public class BlogEntryViewModel
{
	public string Title { get; set; }
	public DateTime PostDate { get; set; }

	[UmbracoRichTextProperty(alias="bodyContent")]
	public string Content { get; set; }
	
	public MediaItem Image { get; set; }	
}
```

This model can now get injected into our views with our fancy `VaultRenderMvcController`. Your `BlogEntry` view  can now look like this:


```html
  @model BlogEntryViewModel

  <h1>@Model.Title</h1>
  <img src="@Model.Image.Url" alt="@Model.Image.AltText" />
  <div>@Model.PostDate.ToShortDateString()</div>
  <div>@Html.Raw(Model.Content)</div>	

```

Much cleaner, with compile time checking! Reads nicer than the usual `@Umbraco.Field("content")`

Want to learn more? Check out the wiki!

## Nuget Installation

```PowerShell
PM> Install-Package UmbracoVault
```

## Configuration

Create the following class to register your view model namespace, and set the default render MVC controller for Umbraco to Umbraco Vault's default controller.

```csharp
public class CustomApplicationEventHandler : ApplicationEventHandler
{
    protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
    {
        Vault.RegisterViewModelNamespace("MyProject.ViewModels", "MyProject");
        DefaultRenderMvcControllerResolver.Current.SetDefaultControllerType(typeof(VaultRenderMvcController));
    }
}
```

## Extensibility

UmbracoVault was built to be extensible to other Umbraco package developers. What good is an ORM if it doesn't support
other native object types? Please see our documentation on
[how to extend Vault](https://github.com/thenerdery/UmbracoVault/wiki/Extending-Vault) for your own Umbraco packages.

Check out the uComponents extension for an example: <https://github.com/thenerdery/UmbracoVault.uComponents>

## Credits

Huge thanks to The Nerdery for supporting the development effort of this ORM. Additionally, thanks to project
contributors for their effort in building this library:

 * Paul Trandem ([@ptrandem](https://github.com/ptrandem))
 * Andrew Morger ([@asmorger](https://github.com/asmorger))
 * Wade Kallhoff ([@wkallhof](https://github.com/wkallhof))
 * Ken Sykora ([@kensykora](https://github.com/kensykora))
 * Jared Swarts ([@jaredswarts55](https://github.com/jaredswarts55))
 * Marshall Glanzer ([@NerderyMGlanzer](https://github.com/NerderyMGlanzer))
 * Kaleb Luhman ([@kluhman](https://github.com/kluhman))
 * Matt Burke ([@akatakritos](https://github.com/akatakritos))
 * Allison Knauss ([@phillipknauss](https://github.com/phillipknauss))
 * Erik Weiss ([@technicallyerik](https://github.com/technicallyerik))
