using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nerdery.Umbraco.Vault.Exceptions
{
    public class ConstructorUnavailableException : Exception
    {
        public ConstructorUnavailableException(Type type)
        {
            _message = string.Format("Could not create type {0} because a valid constructor was not found. A parameterless constructor is usually appropriate, although in some instances a constructor that accepts an IPublishedContent is also valid.", type.Name);
        }

        private readonly string _message;

        public override string Message
        {
            get { return _message; }
        }
    }
}
