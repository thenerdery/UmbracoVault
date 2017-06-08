using System;
using System.Collections.Generic;
using System.Linq;

using Umbraco.Core.Models;

using UmbracoVault.Attributes;

namespace UmbracoVault.TypeHandlers
{
    [IgnoreTypeHandlerAutoRegistration]
    public class UmbracoEntityTypeHandler : ITypeHandler
    {
        public object GetAsType<T>(object input)
        {
            var publishedContent = input as IPublishedContent;
            var id = publishedContent?.Id.ToString() ?? input?.ToString();

            var content = Vault.Context.GetContentById<T>(id);

            return content;
        }

        public Type TypeSupported => null;
    }
}