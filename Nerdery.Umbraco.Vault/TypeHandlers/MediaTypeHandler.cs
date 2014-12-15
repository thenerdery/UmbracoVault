using System;

using Nerdery.Umbraco.Vault.Exceptions;
using Nerdery.Umbraco.Vault.Extensions;

using Umbraco.Core.Models;
using Umbraco.Web;

namespace Nerdery.Umbraco.Vault.TypeHandlers
{
    /// <summary>
    /// Responsible for converting Media Types
    /// </summary>
    public class MediaTypeHandler : ITypeHandler
    {

        private IPublishedContent Get(string stringValue)
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
                Vault.InternalContext.FillClassProperties(result, (alias, recursive) => mediaObject.GetPropertyValue(alias,recursive));
            }
            return result;
        }

        public Type TypeSupported
        {
            get { return typeof (Media); }
        }
    }
}