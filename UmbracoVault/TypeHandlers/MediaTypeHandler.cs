using System;
using System.Collections.Generic;
using System.Linq;

using Umbraco.Core.Models;
using Umbraco.Web;

namespace UmbracoVault.TypeHandlers
{
    /// <summary>
    ///     Responsible for converting Media Types
    /// </summary>
    public class MediaTypeHandler : ITypeHandler
    {
        private static IPublishedContent Get(string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return null;
            }

            var helper = new UmbracoHelper(UmbracoContext.Current);
            return helper.TypedMedia(stringValue);
        }

        public object GetAsType<T>(object input)
        {
            var mediaObject = input as IPublishedContent;
            mediaObject = mediaObject ?? Get(input?.ToString());

            if (mediaObject == null)
            {
                return null;
            }

            var media = Vault.Context.GetMediaById<T>(mediaObject.Id);

            return media;
        }

        public Type TypeSupported => typeof(Media);
    }
}