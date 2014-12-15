using System;
using System.Collections.Generic;
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

		public Type TypeSupported
		{
			get
			{
				return typeof(IEnumerable<>);
			}
		}
	}
}
