using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nerdery.Umbraco.Vault.TypeHandlers;

namespace Nerdery.Umbraco.Vault.Attributes
{
    /// <summary>
    /// Directs Vault to hydrate a target type from a Media object instead of a Content object 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
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
