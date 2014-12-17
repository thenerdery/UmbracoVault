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

        [UmbracoProperty(Alias="richText")]
        public HtmlString RichTextAsHtmlString { get; set; }

        public HtmlString HtmlString { get; set; }
    }
}