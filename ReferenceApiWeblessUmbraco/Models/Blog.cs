using System;
using System.Collections.Generic;
using System.Linq;
using UmbracoVault.Attributes;

namespace ReferenceApiWeblessUmbraco.Models
{
    [UmbracoMediaEntity(AutoMap = true)]
    public class BlogEntryViewModel : CmsViewModelBase
    {
        public string Title { get; set; }
        public DateTime PostDate { get; set; }
        
        [UmbracoRichTextProperty]
        public string Content { get; set; }

        [UmbracoProperty(Alias = "image")]
        public Image PostImage { get; set; }
    }

    [UmbracoEntity(AutoMap = true)]
    public class BlogHomeViewModel : CmsViewModelBase
    {
        public string Title { get; set; }
        public string SidebarCopy { get; set; }
        public BlogEntryViewModel FeaturedBlogEntry { get; set; }
    }
    
}