using System;
using System.Collections.Generic;
using System.Linq;

using Nerdery.Umbraco.Vault.TypeHandlers;

namespace Nerdery.Umbraco.Vault.Attributes
{
    /// <summary>
    /// Denotes an entity represents an Umbraco Media type.
    /// Exposes the same options as [UmbracoEntity], including AutoMap.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class UmbracoMediaEntityAttribute : UmbracoEntityAttribute
    {
        public override Type TypeHandlerOverride
        {
            get
            {
                return typeof(MediaTypeHandler); 
            }
            set
            {
                throw new NotSupportedException("Cannot override type for UmbracoMedia type handler");
            }
        }
    }
}
