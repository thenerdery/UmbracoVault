using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UmbracoVault.Exceptions
{
    public class ViewModelNotFoundException : Exception
    {
        public ViewModelNotFoundException(string templateName, IEnumerable<string> typesChecked)
        {
            var sb = new StringBuilder();
            sb.Append($"The view model inferred from the template ({templateName}) could not be found in the following namespaces:\n");
            foreach (var type in typesChecked)
            {
                sb.AppendLine(type);
            }
            _message = sb.ToString();
        }

        private readonly string _message;

        public override string Message => _message;
    }
}
