## v1.2.0 (UNRELEASED)
 
**New Feature: Lazy Loading**
 
Umbraco Vault now includes support for lazy loading of properties. Properties that are expensive to hydrate,
such as collections, can be marked as `virtual` and they will be loaded when accessed. See <TBD> article
for more detail on this feature.

**Issues**
 
 * Fixed issue where exception would be thrown if loading an assembly threw an error. Added try/catch around logic and tracing any issues identified.

**Other**

 * Codebase has been updated to build against VS 2015 and C# 6
 * Reference sites for testing builds have been updated to the latest v6 and v7 versions, including ASP.Net MVC5
 * Various internal accessiblity changes have been made to limit exposure of the API
 * Various code cleanup
 
## v1.1.1

 * The new base controller expected to be uesd with Umbraco has been renamed from `VaultDefaultGenericController` to `VaultRenderMvcController` in order to more closely
   Umbraco conventions
 * VaultDefaultGenericController and VaultDefaultRenderController have been marked as obsolete.

## v1.1.0

 * Added support for Member types in the CMS (thanks @akatakritos) [#2][#3][#4]
 * Fixed issue with running reference site for Umbraco v7 [#1]
 * Fixed issue with rendering rich text fields that contained macros when there is no current page context (e.g., surface controller) (thanks @NerderyMGlanzer) [#5]

## v1.0.1

 * Fixed issue with nuget spec incorrectly depending on Umbraco v6 only

## Initial Release (1.0)

 * Includes support for Umbraco 6 and 7