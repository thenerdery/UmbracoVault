## v1.3.0 (UNRELEASED)

**New Feature: Independence from Web Context**

Extended the current UmbracoContext implementation to include a secondary implementation that removes any dependency on `IPublishedContext`, `UmbracoHelper`, and the Umbraco WebContext.  Instead of using the WebContext as a vehicle for retrieving data from Umbraco, the [ServiceContext](https://our.umbraco.org/documentation/Reference/Management/Services/) is used. [#27]

**Other**
 
 * Added `PropertyInfo` parameter to `FillClassProperties` to support additional behavior for libraries that extend Vault.  [#25]

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
