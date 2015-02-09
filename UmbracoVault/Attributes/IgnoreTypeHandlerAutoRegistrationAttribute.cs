using System;

namespace UmbracoVault.Attributes
{
    /// <summary>
    /// Attribute that signifies that a TypeHandler should not be automatically loaded into the master list of type handlers.
    /// This attribute is to be used on implementations of ITypeHandler that are explicitly used by a custom UmbracoProperty attribute.
    /// This tells the TypeHandlerFactory to not load this ITypeHandler implementation into the default list.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class IgnoreTypeHandlerAutoRegistrationAttribute : Attribute
    {

    }
}