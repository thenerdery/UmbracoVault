## v1.3.7
* Allows media entities to override type handler so Umbraco 7 users can support UDI format

## v1.3.6
* Fixes issues with enumerable content handling

## v1.3.5
* Automatically mapping media and content entities

## v1.3.3
* Updates MediaTypeHandler to return null if no media is found rather than returning an object with all null properties

## v1.3.2

* Updates UmbracoWebContext.GetCurrent to use the PublishedContent from the PublishedContentRequest rather than getting a page by the current id. 
* Updated value transformation process to execute type handler even if value type already matches target type to ensure all proper transforms are applied.

## v1.3.1

* Fixes issue where proxied models created from IContent models did not support recursive hydration.
* Fixes issue where if a proxy property was overwritten with some value not from CMS, it would not persist and would still use CMS value.
* Fixes issue where preview content breaks due to use of a singleton UmbracoHelper. [#34]

## v1.3.0

**New Feature: Independence from Web Context**

Extended the current UmbracoContext implementation to include a secondary implementation that removes any dependency on `IPublishedContext`, `UmbracoHelper`, and the Umbraco WebContext.  Instead of using the WebContext as a vehicle for retrieving data from Umbraco, the [ServiceContext](https://our.umbraco.org/documentation/Reference/Management/Services/) is used. [#27]

**Other**
 
 * Added `PropertyInfo` parameter to `FillClassProperties` to support additional behavior for libraries that extend Vault.  [#25]

## v1.2.3
 * Fixes an issue where high amount of concurrent threads would cause multiple entries with same key to proxy property cache

## v1.2.2
 * Fixes an issue where models implementing an interface would not properly have their values set [#29]
 * Fixes an issue where explicit interface implementations would not properly have their values set [#29]
>>>>>>> master

## v1.2.1

 * Fixes an issue where classes that used type inheritance would cause duplicate results to return in calls to `GetUmbracoEntityAliasesFromType` [#12]

## v1.2.0

**New Feature: Lazy Loading (#6)**
 
Umbraco Vault now includes support for lazy loading of properties. Properties that are expensive to hydrate,
such as collections, can be marked as `virtual` and they will be loaded when accessed. See <https://github.com/thenerdery/UmbracoVault/wiki/Lazy-property-loading> article
for more detail on this feature.

**New Type Handlers**

 * Vault now supports nullable types for primitives and structs. See <https://github.com/thenerdery/UmbracoVault/wiki/Umbraco-Data-Type---C%23-Data-Type-Grid#nullable-data-types> for more detail.
 * Vault now supports mapping from JSON data stored within properties. See <https://github.com/thenerdery/UmbracoVault/wiki/Umbraco-Data-Type---C%23-Data-Type-Grid#json-data> for more detail.

**Issues**
 
 * Fixed issue where exception would be thrown if loading an assembly threw an error. Added try/catch around logic and tracing any issues identified. [#11]
 * Fixed issue if an external assembly attempts to register a type already handled, that an exception would be thrown. A trace warning is now written instead.

**Other**

 * Codebase has been updated to build against VS 2015 and C# 6
 * Reference sites for testing builds have been updated to the latest v6 and v7 versions, including ASP.Net MVC5
 * Various code cleanup

This is a pretty significant release! Thanks to @NerderyMGlanzer, @technicallyerik, and @jesse-black for their contributions!
 
## v1.1.1

 * The new base controller expected to be uesd with Umbraco has been renamed from `VaultDefaultGenericController` to `VaultRenderMvcController` in order to more closely
   Umbraco conventions
 * `VaultDefaultGenericController` and `VaultDefaultRenderController` have been marked as obsolete.

## v1.1.0

 * Added support for Member types in the CMS (thanks @akatakritos) [#2][#3][#4]
 * Fixed issue with running reference site for Umbraco v7 [#1]
 * Fixed issue with rendering rich text fields that contained macros when there is no current page context (e.g., surface controller) (thanks @NerderyMGlanzer) [#5]

## v1.0.1

 * Fixed issue with nuget spec incorrectly depending on Umbraco v6 only

## Initial Release (1.0)

 * Includes support for Umbraco 6 and 7
