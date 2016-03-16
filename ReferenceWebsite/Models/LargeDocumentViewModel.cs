using System;
using System.Collections.Generic;
using System.Linq;

using UmbracoVault.Attributes;

namespace ReferenceWebsite.Models
{
    [UmbracoEntity(AutoMap = true, Alias = "Largedocument")]
    public class LargeDocumentViewModel : CmsViewModelBase
    {
        public virtual string Text { get; set; }
        public virtual string Text2 { get; set; }
        public virtual string Text3 { get; set; }
        public virtual string Text4 { get; set; }
        public virtual string Text5 { get; set; }
        public virtual int ContentPicker { get; set; }
        public virtual int ContentPicker2 { get; set; }
        public virtual Image MediaPicker { get; set; }
        public virtual string[] CustomColorSelect { get; set; }
        public virtual string[] CustomColorSelect2 { get; set; }

        /// <summary>
        ///     Raw integer arrays are supported using a textstring
        /// </summary>
        public virtual int[] IntArray { get; set; }

        /// <summary>
        ///     Raw string arrays are supported using a textstring
        /// </summary>
        public virtual string[] StringArray { get; set; }

        /// <summary>
        ///     Generic content lists are supported.
        /// </summary>
        public virtual IList<StaffMember> StaffList { get; set; }

        /// <summary>
        ///     This contains the list of Checkbox Values
        /// </summary>
        public virtual string[] CheckboxList { get; set; }

        /// <summary>
        ///     THis contains the list of Dictionary Picker values
        /// </summary>
        public virtual string[] DictionaryPicker { get; set; }

        /// <summary>
        ///     List of Integers that correspond to prevalues in the Umbraco DB. Lookup is required to get text values.
        /// </summary>
        public virtual int DropDownListMultiplePublishKeys { get; set; }

        /// <summary>
        ///     List of dropdown values. Publishes the string entry so no lookup is required
        /// </summary>
        public virtual string[] DropDownListMultiple { get; set; }
    }
}