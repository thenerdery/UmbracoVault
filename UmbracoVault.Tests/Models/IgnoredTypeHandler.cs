using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmbracoVault.Attributes;
using UmbracoVault.TypeHandlers;

namespace UmbracoVault.Tests.Models
{
    [IgnoreTypeHandlerAutoRegister]
    class IgnoredTypeHandler :ITypeHandler
    {
        public object GetAsType<T>(object input)
        {
            throw new NotImplementedException();
        }

        public Type TypeSupported { get { return typeof (IgnoredType); } }
    }

    public class IgnoredType { }
}
