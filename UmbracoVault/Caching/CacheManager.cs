using System;
using System.Configuration;
using System.Runtime.Caching;

namespace UmbracoVault.Caching
{
    public class CacheManager
    {
        public virtual object GetItem<T>(int id)
        {
            var cacheKey = $"{typeof(T).Name}_{id}";
            var item = MemoryCache.Default.Get(cacheKey);
            return item;
        }

        public virtual void AddItem<T>(int id, T value)
        {
            var cacheKey = $"{typeof(T).Name}_{id}";
            var expiration = DateTime.Now.AddSeconds(GetCacheSeconds());

            var cacheItem = new CacheItem(cacheKey, value);
            var cachePolicy = new CacheItemPolicy { AbsoluteExpiration = expiration };

            MemoryCache.Default.Add(cacheItem, cachePolicy);
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
