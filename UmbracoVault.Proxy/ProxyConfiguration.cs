using System;
using System.Collections.Generic;
using System.Linq;

using UmbracoVault.Proxy.Concrete;

namespace UmbracoVault.Proxy
{
    public static class ProxyConfiguration
    {
        public static void EnableProxies()
        {
            var factory = new ProxyFactory();
            ClassConstructor.SetInstanceFactory(factory);
        }
    }
}