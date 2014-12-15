using System.Collections.Generic;
using UmbracoVault.Attributes;

namespace ReferenceWebsite.Models
{
    [UmbracoEntity(AutoMap = true)]
    public class Arrays
    {
        /// <summary>
        /// Raw integer arrays are supported using a textstring
        /// </summary>
        public int[] IntArray { get; set; }

        /// <summary>
        /// Raw string arrays are supported using a textstring
        /// </summary>
        public string[] StringArray { get; set; }

        /// <summary>
        /// Generic content lists are supported.
        /// </summary>
        public IList<StaffMember> StaffList { get; set; } 
    }
}