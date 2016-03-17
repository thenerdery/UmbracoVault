using System;
using System.Collections.Generic;
using System.Linq;

namespace UmbracoVault.Exceptions
{
    class TemplateFileNotFoundException : Exception
    {
        private readonly string _message;
        public TemplateFileNotFoundException(string templateName)
        {
            _message = $"A template file for the template ({templateName}) was not found on disk.";
        }

        public override string Message => _message;
    }
}
