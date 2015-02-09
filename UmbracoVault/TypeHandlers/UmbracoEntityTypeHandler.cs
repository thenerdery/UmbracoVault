using System;
using System.Collections.Generic;
using System.Linq;

using UmbracoVault.Attributes;

namespace UmbracoVault.TypeHandlers
{
    [IgnoreTypeHandlerAutoRegistration]
    public class UmbracoEntityTypeHandler : ITypeHandler
    {
        public object GetAsType<T>(object input)
        {
            var content = Vault.Context.GetContentById<T>(input.ToString());

            return content;
        }

        public Type TypeSupported {
            get { return null; }
        }
    }
}
