using System;
using System.Collections.Generic;
using System.Linq;

using Umbraco.Core.Models;
using Umbraco.Web;

using UmbracoVault.Attributes;
using UmbracoVault.Exceptions;
using UmbracoVault.Extensions;

namespace UmbracoVault.TypeHandlers
{
    [IgnoreTypeHandlerAutoRegistration]
    public class UmbracoEntityTypeHandler : ITypeHandler
    {
        public object GetAsType<T>(object input)
        {
            var publishedContent = input as IPublishedContent;
            if (publishedContent != null)
            {
                var result = typeof(T).CreateWithContentConstructor<T>(publishedContent);
                if (result == null)
                {
                    result = typeof(T).CreateWithNoParams<T>();
                }

                if (result == null)
                {
                    throw new ConstructorUnavailableException(typeof(T));
                }

                Vault.Context.FillClassProperties(result, (alias, recursive) => publishedContent.GetPropertyValue(alias, recursive));

                return result;
            }

            var content = Vault.Context.GetContentById<T>(input.ToString());

            return content;
        }

        public Type TypeSupported => null;
    }
}
