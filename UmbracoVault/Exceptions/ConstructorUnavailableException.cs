using System;
using System.Collections.Generic;
using System.Linq;

namespace UmbracoVault.Exceptions
{
    public class ConstructorUnavailableException : Exception
    {
        public ConstructorUnavailableException(Type type)
        {
            _message = $"Could not create type {type.Name} because a valid constructor was not found. A parameterless constructor is usually appropriate, although in some instances a constructor that accepts an IPublishedContent is also valid.";
        }

        private readonly string _message;

        public override string Message => _message;
    }
}
