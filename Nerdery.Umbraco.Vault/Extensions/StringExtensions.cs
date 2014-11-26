using System;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using umbraco;
using umbraco.presentation.templateControls;

namespace Nerdery.Umbraco.Vault.Extensions
{
    public static class StringExtensions
    {
		/// <summary>
		/// Given a string value, will convert it into an enum value
		/// </summary>
		public static T ConvertToEnum<T>(this string value)
		{
			Contract.Requires(typeof(T).IsEnum);
			Contract.Requires(value != null);
			Contract.Requires(Enum.IsDefined(typeof(T), value));
			return (T)Enum.Parse(typeof(T), value);
		}
		/// <summary>
		/// Given rich text content, will render the internal links
		/// </summary>
		public static string RenderLinks(this string richText)
		{
			// take a rich text string, parse the local links

			Regex ll = new Regex("(/{localLink:)(.*?)}");

			MatchEvaluator myEvaluator = new MatchEvaluator(MatchLocalLinks);

			var LocalLinksParsed = ll.Replace(richText, myEvaluator);
			return HttpUtility.HtmlDecode(LocalLinksParsed);
		}

		private static string MatchLocalLinks(Match m)
		{
			var noHome = Regex.Replace(umbraco.library.NiceUrl(int.Parse(m.Result("$2"))), ".*/home(.*)", "$2");
			return noHome;
		}
        /// <summary>
        /// Parse an html string for any UMBRACO_MACRO tokens, and replace them with their rendered macros
        /// </summary>
        public static string ProcessUmbracoMacros(this string richtextHtmlWithMacros)
        {
            bool macroFound;
			bool nodeFound = true;
            do
            {
                Match macroMatch = Regex.Match(richtextHtmlWithMacros, @"<\?UMBRACO_MACRO (.*?)macroAlias=""(.*?)""(.*?)/>", RegexOptions.Singleline);
                macroFound = macroMatch.Success;

                if (macroFound)
                {
                    var xmlDocument = new XmlDocument();

                    // remove the '?' char from the token so that can be treated as xml
                    xmlDocument.LoadXml(macroMatch.Value.Remove(1, 1));

                    XmlNode macroXmlNode = xmlDocument.SelectSingleNode("//UMBRACO_MACRO");

                    var macro = new Macro
                                    {
                                        Alias = macroXmlNode.Attributes["macroAlias"].Value
                                    };

                    foreach (XmlAttribute attribute in macroXmlNode.Attributes)
                    {
                        if (attribute.Name != "macroAlias")
                        {
                            macro.MacroAttributes.Add(attribute.Name, attribute.Value);
                        }
                    }

					if (macro.MacroAttributes["widgetcontentsourcenodeid"] != null)
					{
						var id = macro.MacroAttributes["widgetcontentsourcenodeid"] as string;
						var node = uQuery.GetNode(id);
						nodeFound = node != null && node.Id > 0;
					}
                    richtextHtmlWithMacros = richtextHtmlWithMacros.Remove(macroMatch.Index, macroMatch.Length);
					if (nodeFound)
					{
						richtextHtmlWithMacros = richtextHtmlWithMacros.Insert(macroMatch.Index, macro.RenderToString());
					}

                }
            }
            while (macroFound);

            return richtextHtmlWithMacros;
        }
    }
}
