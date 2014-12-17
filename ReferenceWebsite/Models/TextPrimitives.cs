using System.Web;
using UmbracoVault.Attributes;

namespace ReferenceWebsite.Models
{
    [UmbracoEntity(AutoMap = true)]
    public class TextTypesViewModel : CmsViewModelBase
    {
        /// <summary>
        /// A char can hold a single character.
        /// </summary>
        public char Char { get; set; }

        /// <summary>
        /// A string can hold many characters.
        /// </summary>
        public string String { get; set; }

        /// <summary>
        /// Rich text is a string with macros applied.
        /// </summary>
        [UmbracoRichTextProperty]
        public string RichText { get; set; }

        /// <summary>
        /// Uses the RichText property above but converts directly into an HtmlString which
        /// avoids the need for an Html.Raw() call in the view.
        /// </summary>
        [UmbracoProperty(Alias="richText")]
        public HtmlString RichTextAsHtmlString { get; set; }

        /// <summary>
        /// Uses a plain text value to populate this, similar to the example above
        /// </summary>
        public HtmlString HtmlString { get; set; }

        /// <summary>
        /// Gets the value from a drop down list (the actual text from it)
        /// </summary>
        public string DropDownList { get; set; }

        /// <summary>
        /// Uses a drop down list but for this type Umbraco publishes an ID key which
        /// requires a lookup from umbraco.library.GetPreValueAsString(id) to retrieve
        /// the string value
        /// </summary>
        public int DropDownListPublishKeys { get; set; }
    }
}