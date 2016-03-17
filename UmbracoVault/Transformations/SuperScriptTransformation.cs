using System;
using System.Configuration;

namespace UmbracoVault.Transformations
{
    /// <summary>
    /// Takes in an object of type <see cref="System.String"/> and will wrap specific characters with
    /// &lt;sup&gt; tags such that they have superscript around them.
    /// </summary>
    public class SuperScriptTransformation : ITransformation
    {
        private readonly string[] _targetCharacters;

        public SuperScriptTransformation()
        {
            var targetCharactersString = ConfigurationManager.AppSettings["SuperscriptTransform.TargetCharacters"];
            if(!string.IsNullOrWhiteSpace(targetCharactersString))
            {
                _targetCharacters = targetCharactersString.Split(',');
            }
        }

        public Type TypeSupported => typeof (string);

        public object Transform(object input)
        {
            if (_targetCharacters == null || _targetCharacters.Length == 0) return input;
            
            var output = input.ToString();
            foreach(var character in _targetCharacters)
            {
                output = output.Replace(character, $"<sup>{character}</sup>");
            }

            return output;
        }
    }
}
