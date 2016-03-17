using System;

namespace UmbracoVault.Attributes
{
    /// <summary>
    /// Denotes that a property should be ignored Vault in the case that AutoMap is enabled at the class level.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class UmbracoIgnorePropertyAttribute : Attribute
    {
    }
}