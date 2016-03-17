using System;

using UmbracoVault.Exceptions;
using UmbracoVault.Extensions;

using Umbraco.Core.Models;
using Umbraco.Web;

namespace UmbracoVault.TypeHandlers
{
    /// <summary>
    /// Responsible for converting Media Types
    /// </summary>
    public class MediaTypeHandler : ITypeHandler
    {

        private static IPublishedContent Get(string stringValue)
        {
            var helper = new UmbracoHelper(UmbracoContext.Current);
            return helper.TypedMedia(stringValue);
        }

    	public object GetAsType<T>(object input)
    	{
            var result = typeof(T).CreateWithNoParams<T>();

            if (result == null)
            {
                throw new ConstructorUnavailableException(typeof(T));
            }

            var mediaObject = Get(input.ToString());
            if (mediaObject != null)
            {
                Vault.Context.FillClassProperties(result, (alias, recursive) => mediaObject.GetPropertyValue(alias,recursive));
            }
            return result;
        }

        public Type TypeSupported => typeof (Media);
    }
}