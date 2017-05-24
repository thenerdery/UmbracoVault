using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using umbraco;
using umbraco.presentation.templateControls;

namespace UmbracoVault.Extensions
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

            var ll = new Regex("(/{localLink:)(.*?)}");

            var myEvaluator = new MatchEvaluator(MatchLocalLinks);

            var localLinksParsed = ll.Replace(richText, myEvaluator);
            return HttpUtility.HtmlDecode(localLinksParsed);
        }

        private static string MatchLocalLinks(Match m)
        {
            var noHome = Regex.Replace(library.NiceUrl(int.Parse(m.Result("$2"))), ".*/home(.*)", "$2");
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
                        // ReSharper disable PossibleNullReferenceException
                        Alias = macroXmlNode.Attributes["macroAlias"].Value
                        // ReSharper restore PossibleNullReferenceException
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

        internal static string ToPascalCase(this string source)
        {
            return ToPascalCase(source, CultureInfo.CurrentCulture);
        }

        internal static string ToPascalCase(this string source, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }

            return string.Join("", source.Split().Select(s => SingleStringToPascalCase(s, culture)));
        }

        private static string SingleStringToPascalCase(string s, CultureInfo culture)
        {
            return string.IsNullOrEmpty(s)
                ? s
                : new string(s.Select((c, i) => i == 0 ? char.ToUpper(c, culture) : c)
                              .ToArray());
        }

        internal static string ToCamelCase(this string source)
        {
            return ToCamelCase(source, CultureInfo.CurrentCulture);
        }

        internal static string ToCamelCase(this string source, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }

            return string.Join("", source.Split().Select((s,i) => i == 0 ? SingleStringToCamelCase(s, culture) : s));
        }

        private static string SingleStringToCamelCase(string s, CultureInfo culture)
        {
            return string.IsNullOrEmpty(s)
                ? s
                : new string(s.Select((c, i) => i == 0 ? char.ToLower(c, culture) : c)
                              .ToArray());
        }
    }
}
