using System;

namespace UmbracoVault.Attributes
{
    /// <summary>
    /// Attribute that signifies that this assembly contains UmbracoVault type handlers. 
    /// Vault should attempt to register them.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class ContainsUmbracoVaultTypeHandlersAttribute : Attribute
    {

    }
}