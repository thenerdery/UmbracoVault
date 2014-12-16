#Vault for Umbraco 6

### Build Status:

| Branch | Status |
| ------ | ------ |
| master | [![Build status](https://ci.appveyor.com/api/projects/status/bqbyg6yevctgm7tl?svg=true)](https://ci.appveyor.com/project/kensykora/umbracovault) |
| develop | [![Build status](https://ci.appveyor.com/api/projects/status/bqbyg6yevctgm7tl/branch/develop?svg=true)](https://ci.appveyor.com/project/kensykora/umbracovault/branch/develop) |

**Version 0.9, Designed for Umbraco 6.x**

***Note**: We're already hard at work developing and testing a more refined version of Vault designed to support Umbraco 7. Some features identified in this document may not apply to newer versions of Umbraco Vault. But don't worry, the good stuff will stick around.*

***Also note**: Vault has a lot of features. We're actively working on documenting all of them, but this may take some time; this document is not yet complete.*

##Overview
Vault for Umbraco is an easy-to-use, extensible ORM to quickly and easily get strongly-typed Umbraco CMS data into your markup.  It allows you to create lightly-decorated classes that Vault will understand how to hydrate. This gives you the full view model-style experience in Umbraco that you are accustomed to in MVC, complete with strongly-typed view properties (no more magic strings in your views).

##The Idea

let's assume we have a document type with the alias `BlogEntry` set up with the following properties:

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

	[UmbracoRichTextProperty(alias="bodyContent")]
	public string Content { get; set; }
	
	public MediaItem Image { get; set; }	
}
```

This model can now get injected into our views with our fancy `VaultDefaultGenericController`. Now our views look like this:


```
  @inherits Umbraco.Web.Mvc.UmbracoTemplatePage

  <h1>@Model.Title</h1>
  <img src="@Model.Image.Url" alt="@Model.Image.AltText" />
  <div>@Model.PostDate.ToShortDateString()</div>
  <div>@Html.Raw(Model.Content)</div>	

```

Much cleaner, with compile time checking. Reads nicer than the usual `@Umbraco.Field("bodyContent")`

Want to learn more? Check out the wiki!

##Nuget Installation

```
PM> Install-Package UmbracoVault -IncludePrerelease -Source https://ci.appveyor.com/nuget/umbracovault-5m6ate96gcwx  -UserName <appveyoremail> -Password <appveyorpassword>
```

## Extensibility

UmbracoVault was built to be extensible to other Umbraco package developers. What good is an ORM if it doesn't support other native object types? Please see our documentation on [how to extend Vault](https://github.com/kensykora/UmbracoVault/wiki/Extending-Vault) for your own Umbraco packages.

## Credits

Huge thanks to The Nerdery for supporting the development effort of this ORM. Additionally, thanks to project contributors for their effort in building this library:

 * Paul Trandem
 * Andrew Morger
 * Ken Sykora
 * Jared Swarts
 * Phillip Allen Knauss