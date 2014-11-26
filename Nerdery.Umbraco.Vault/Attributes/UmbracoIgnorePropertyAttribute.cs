using System;

namespace Nerdery.Umbraco.Vault.Attributes
{
    /// <summary>
    /// Denotes that a property should be ignored Vault in the case that AutoMap is enabled at the class level.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UmbracoIgnorePropertyAttribute : Attribute
    {
    }
}