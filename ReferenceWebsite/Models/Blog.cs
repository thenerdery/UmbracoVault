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
    
}