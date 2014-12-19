using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmbracoVault.TypeHandlers;

namespace UmbracoVault.Tests.Models
{
    public class AutoRegisteredTypeHandler : ITypeHandler
    {
        public object GetAsType<T>(object input)
        {
            throw new NotImplementedException();
        }

        public Type TypeSupported { get { return typeof (AutoRegisteredType); } }
    }

    public class AutoRegisteredType { }
}
