using System;
using System.Collections.Generic;
using System.Reflection;

namespace Nerdery.Umbraco.Vault.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static IEnumerable<PropertyInfo> GetPublicSettableProperties(this Type type)
        {
            return type.GetProperties(BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance);
        }
    }
}
