using System;
using System.Collections.Generic;
using System.Linq;

using UmbracoVault.TypeHandlers;

namespace UmbracoVault.Attributes
{
    /// <summary>
    /// Directs Vault to hydrate a target type from a Media object instead of a Content object 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class UmbracoMediaPropertyAttribute : UmbracoPropertyAttribute
    {
        public UmbracoMediaPropertyAttribute()
        {
            TypeHandler = new MediaTypeHandler();
        }

        /// <summary>
        /// Only needed if the camelCase name of the property is not the same as the umbraco property alias
        /// </summary>
        /// <param name="name"></param>
        public UmbracoMediaPropertyAttribute(string name)
            : base(name)
        {
            TypeHandler = new MediaTypeHandler();
        }
    }
}
