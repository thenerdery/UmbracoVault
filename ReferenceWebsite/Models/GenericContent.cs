using UmbracoVault.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReferenceWebsite.Models
{
    [UmbracoEntity(AutoMap=true)]
    public class GenericContentViewModel
    {
        public string Content { get; set; }
    }
}