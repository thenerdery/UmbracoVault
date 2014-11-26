using System.IO;
using System.Text;
using System.Web.UI;

namespace Nerdery.Umbraco.Vault.Extensions
{
    public static class ControlExtensions
    {
        /// <summary>
        /// Renders an ASP.NET control into a string
        /// </summary>
        public static string RenderToString(this Control control)
        {
            StringBuilder stringBuilder = new StringBuilder();
            StringWriter stringWriter = new StringWriter(stringBuilder);
            HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);

            control.RenderControl(htmlTextWriter);

            return htmlTextWriter.InnerWriter.ToString();
        }
    }
}
