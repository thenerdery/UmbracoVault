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
            _message = string.Format("A template file for the template ({0}) was not found on disk.", templateName);
        }

        public override string Message
        {
            get { return _message; }
        }
    }
}
