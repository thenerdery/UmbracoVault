using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UmbracoVault.Proxy.Concrete;

namespace UmbracoVault.Proxy
{
    public static class ProxyConfiguration
    {
        public static void EnableProxies()
        {
            var factory = new ProxyFactory();
            ClassConstructor.SetInstanceFactory();
        }
    }
}
