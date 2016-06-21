using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;

namespace ReferenceApiWeblessUmbraco.Models
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
        public IPublishedContent CmsContent { get; set; }
    }
}