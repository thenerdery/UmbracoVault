using System.Collections.Generic;
using System.Linq;
using UmbracoVault.Attributes;

namespace ReferenceApiWeblessUmbraco.Models
{
    [UmbracoEntity(AutoMap=true)]
    public class GenericContentViewModel
    {
        public string Content { get; set; }
    }
}