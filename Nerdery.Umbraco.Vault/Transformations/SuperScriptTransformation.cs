using System;
using System.Configuration;

namespace Nerdery.Umbraco.Vault.Transformations
{
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

        public Type TypeSupported
        {
            get { return typeof (string); }
        }

        public object Transform(object input)
        {
            if (_targetCharacters == null || _targetCharacters.Length == 0) return input;
            
            var output = input.ToString();
            foreach(var character in _targetCharacters)
            {
                output = output.Replace(character, string.Format("<sup>{0}</sup>", character));
            }

            return output;
        }
    }
}
