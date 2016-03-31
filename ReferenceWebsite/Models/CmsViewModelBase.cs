using Umbraco.Core.Models;
using Umbraco.Web.Models;
using UmbracoVault.Attributes;

namespace ReferenceWebsite.Models
{
    public interface ICmsViewModel
    {
        IPublishedContent CmsContent { get; set; }
    }

    /// <summary>
    /// Base class for view models, provides Umbraco node information
    /// </summary>
    public class CmsViewModelBase : ICmsViewModel
    {
        [UmbracoProperty(Recursive = true)]
        public virtual string SomeRecursiveValue { get; set; }
        public IPublishedContent CmsContent { get; set; }
    }
}