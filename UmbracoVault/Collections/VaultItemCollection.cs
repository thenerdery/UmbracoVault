using System.Collections.Generic;
using System.Linq;

namespace UmbracoVault.Collections
{
    /// <summary>
    /// Will Eager-load a collection; This should be used with care; For lazy-loading, use IEnumerable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class VaultItemCollection<T> : List<T>
    {
        public VaultItemCollection(string commaDelimitedIds)
        {
            var idValues = commaDelimitedIds;
            if (string.IsNullOrWhiteSpace(idValues))
            {
                return;
            }

            var ids = idValues.Split(',');
            var loadAsMedia = Vault.Context.IsMediaRequest<T>();

            if (loadAsMedia)
            {
                foreach (var item in ids.Select(id => Vault.Context.GetMediaById<T>(id)).Where(item => item != null))
                {
                    Add(item);
                }
            }
            else
            {
                foreach (var item in ids.Select(id => Vault.Context.GetContentById<T>(id)).Where(item => item != null))
                {
                    Add(item);
                }
            }
        }
    }
}
