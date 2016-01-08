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