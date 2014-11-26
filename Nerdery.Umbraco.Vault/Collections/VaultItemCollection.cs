using System.Collections.Generic;

namespace Nerdery.Umbraco.Vault.Collections
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
            var loadAsMedia = Vault.InternalContext.IsMediaRequest<T>();

            if (loadAsMedia)
            {
                foreach (var id in ids)
                {
                    var item = Vault.Context.GetMediaById<T>(id);
                    if (item != null)
                    {
                        Add(item);
                    }
                }
            }
            else
            {
                foreach (var id in ids)
                {
                    var item = Vault.Context.GetContentById<T>(id);
                    if (item != null)
                    {
                        Add(item);
                    }
                }
            }
        }
    }
}
