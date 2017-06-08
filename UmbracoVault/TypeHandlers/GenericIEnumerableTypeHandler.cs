using System;
using System.Collections.Generic;
using System.Linq;

using Umbraco.Core.Models;

using UmbracoVault.Collections;

namespace UmbracoVault.TypeHandlers
{
	/// <summary>
	/// Responsible for converting Generic IEnumerable collections
	/// Expands IDs into Vault objects (of type T) from the context as they are iterated upon
	/// </summary>
	public class GenericIEnumerableTypeHandler : ITypeHandler
	{
		public object GetAsType<T>(object input)
        {
            if (input == null)
            {
                return null;
            }

            var publishedContent = input as IEnumerable<IPublishedContent>;
            if (publishedContent != null)
            {
                return publishedContent.Select(c =>
                {
                    try
                    {
                        return Vault.Context.IsMediaRequest<T>()
                            ? Vault.Context.GetMediaById<T>(c.Id)
                            : Vault.Context.GetContentById<T>(c.Id);
                    }
                    catch (ArgumentException)
                    {
                        return default(T);
                    }
                }).ToList();
            }

			var collection = new ExternalIteratorEnumerable<T>(input.ToString(), idString =>
			    {
			        try
			        {
			            return Vault.Context.IsMediaRequest<T>()
                            ? Vault.Context.GetMediaById<T>(idString)
                            : Vault.Context.GetContentById<T>(idString);
			        }
			        catch (ArgumentException)
			        {
			            return default(T);
			        }
			    });
			return collection;
		}

		public Type TypeSupported => typeof(IEnumerable<>);
	}
}
