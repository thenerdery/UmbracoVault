using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nerdery.Umbraco.Vault.Attributes;
using Nerdery.Umbraco.Vault.Exceptions;
using Nerdery.Umbraco.Vault.Extensions;

namespace Nerdery.Umbraco.Vault.TypeHandlers
{
    [IgnoreAutoLoad]
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
