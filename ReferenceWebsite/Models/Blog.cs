using System;
using Nerdery.Umbraco.Vault.Attributes;

namespace ReferenceWebsite.Models
{
    [UmbracoMediaEntity(AutoMap = true)]
    public class BlogEntryViewModel
    {
        public string Title { get; set; }
        public DateTime PostDate { get; set; }
        
        [UmbracoRichTextProperty]
        public string Content { get; set; }
    }

    [UmbracoEntity(AutoMap = true)]
    public class BlogHomeViewModel
    {
        public string Title { get; set; }
        public string SidebarCopy { get; set; }
        public BlogEntryViewModel FeaturedBlogEntry { get; set; }
    }
    
}