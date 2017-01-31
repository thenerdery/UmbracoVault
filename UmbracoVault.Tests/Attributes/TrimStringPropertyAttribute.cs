using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UmbracoVault.Attributes;
using UmbracoVault.Tests.Handlers;

namespace UmbracoVault.Tests.Attributes
{
    public class TrimStringPropertyAttribute : UmbracoPropertyAttribute
    {
        public TrimStringPropertyAttribute()
        {
            TypeHandler = new TrimStringTypeHandler();
        }

        public TrimStringPropertyAttribute(string alias) : base(alias)
        {
            TypeHandler = new TrimStringTypeHandler();
        }
    }
}
