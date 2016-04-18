using System.Collections.Generic;
using System.Linq;
using UmbracoVault.Attributes;

namespace ReferenceApiWeblessUmbraco.Models
{
    [UmbracoEntity(AutoMap = true)]
    public class ArraysViewModel : CmsViewModelBase
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

        /// <summary>
        /// This contains the list of Checkbox Values
        /// </summary>
        public string[] CheckboxList { get; set; }

        /// <summary>
        /// THis contains the list of Dictionary Picker values
        /// </summary>
        public string[] DictionaryPicker { get; set; }

        /// <summary>
        /// List of Integers that correspond to prevalues in the Umbraco DB. Lookup is required to get text values.
        /// </summary>
        public int[] DropDownListMultiplePublishKeys { get; set; }

        /// <summary>
        /// List of dropdown values. Publishes the string entry so no lookup is required
        /// </summary>
        public string[] DropDownListMultiple { get; set; }
    }
}