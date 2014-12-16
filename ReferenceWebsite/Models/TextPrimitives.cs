using System.Web;
using UmbracoVault.Attributes;

namespace ReferenceWebsite.Models
{
    [UmbracoEntity(AutoMap = true)]
    public class TextPrimitives
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

        public HtmlString HtmlString { get; set; }
    }
}