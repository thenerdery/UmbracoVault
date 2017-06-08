using System;
using System.Collections.Generic;
using System.Linq;

using UmbracoVault.TypeHandlers;

namespace UmbracoVault.Attributes
{
    /// <summary>
    /// Denotes an entity represents an Umbraco Media type.
    /// Exposes the same options as [UmbracoEntity], including AutoMap.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class UmbracoMediaEntityAttribute : UmbracoEntityAttribute
    {
        private Type _typeHandlerOverride = typeof(MediaTypeHandler);

        public override Type TypeHandlerOverride
        {
            get { return _typeHandlerOverride; }
            set
            {
                if (!typeof(MediaTypeHandler).IsAssignableFrom(value))
                {
                    throw new NotSupportedException("Type handler override must inherit media type handler");
                }

                _typeHandlerOverride = value;
            }
        }
    }
}
