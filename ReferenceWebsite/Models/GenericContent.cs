using Nerdery.Umbraco.Vault.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReferenceWebsite.Models
{
    [UmbracoEntity(AutoMap=true)]
    public class GenericContent
    {
        public string Content { get; set; }
    }
}