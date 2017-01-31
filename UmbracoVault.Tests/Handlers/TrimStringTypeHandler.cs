using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UmbracoVault.TypeHandlers;

namespace UmbracoVault.Tests.Handlers
{
    class TrimStringTypeHandler : ITypeHandler
    {
        public object GetAsType<T>(object input)
        {
            return input?.ToString()?.Trim();
        }

        public Type TypeSupported => typeof(string);
    }
}
