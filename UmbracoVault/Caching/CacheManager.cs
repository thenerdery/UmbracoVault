using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace UmbracoVault.Caching
{
    public class CacheManager
    {
        public virtual object GetItem<T>(int id)
        {
            var cacheKey = $"{typeof(T).Name}_{id}";
            var item = HttpContext.Current.Cache.Get(cacheKey);
            return item;
        }

        public virtual void AddItem<T>(int id, T value)
        {
            var cacheKey = $"{typeof(T).Name}_{id}";
            var expiration = DateTime.Now.AddSeconds(GetCacheSeconds());
            HttpContext.Current.Cache.Insert(cacheKey, value, null, expiration, Cache.NoSlidingExpiration);
        }

        private static int GetCacheSeconds()
        {
            var configValue = ConfigurationManager.AppSettings.Get("Vault.ObjectCacheSeconds");
            int result;
            if(!int.TryParse(configValue, out result))
            {
                return 0;
            }
            return result;
        }
    }
}
