using System;
using System.Collections.Generic;
using System.Linq;

using Umbraco.Core.Models;
using Umbraco.Web;

namespace Nerdery.Umbraco.Vault.Extensions
{
    public static class ContentExtensions
    {
        /// <summary>
        /// Finds content based on content alias. 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public static IEnumerable<IPublishedContent> GetContentByAlias(this UmbracoHelper helper, string alias)
        {
            return helper.TypedContentAtXPath(String.Format("//{0}", alias));
        }

        /// <summary>
        /// Gets the home content item for the given Umbraco context.
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static IPublishedContent GetHomeContent(this UmbracoHelper helper)
        {
            return helper.AssignedContentItem.AncestorOrSelf(1);
        }

        /// <summary>
        /// Gets content based on the name in the CMS. 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="name">Name of content to find</param>
        /// <returns></returns>
        public static IEnumerable<IPublishedContent> GetContentByName(this UmbracoHelper helper, string name)
        {
            return helper.GetHomeContent().DescendantsOrSelf().Where(x => x.Name.Equals(name));
        }

        /// <summary>
        /// Returns the strongly typed content item based on the item's URL.
        /// Uses the home node to traverse descendants to find matching URL.
        /// </summary>
        /// <param name="url">URL to return content for</param>
        /// <returns>Published content based on provided URL</returns>
        public static IPublishedContent GetTypedContentByUrl(this UmbracoHelper helper, string url)
        {
            var root = helper.GetHomeContent();
            return root.DescendantsOrSelf().FirstOrDefault(x => x.Url.Equals(url, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Returns the strongly typed content item based on the item's URL.
        /// Uses the home node to traverse descendants to find matching URL.
        /// </summary>
        /// <param name="url">URL to return content for</param>
        /// <returns>Published content based on provided URL</returns>
        public static IEnumerable<IPublishedContent> GetTypedContentByCsv(this UmbracoHelper helper, string csv)
        {
            var ids = csv.Split(',');
            var contents = helper.TypedContent(ids);
            return contents;
        }
    }
}
