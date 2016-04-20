using System;
using UmbracoVault.Attributes;

namespace UmbracoVault.Models
{
    internal class VaultEntity
    {
        public UmbracoEntityAttribute MetaData { get; set; }
        public Type Type { get; set; }
    }
}
