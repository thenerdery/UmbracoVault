using System;
using System.Collections.Generic;
using Nerdery.Umbraco.Vault.Collections;

namespace Nerdery.Umbraco.Vault.TypeHandlers
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
			            return Vault.InternalContext.IsMediaRequest<T>()
                            ? Vault.Context.GetMediaById<T>(idString)
                            : Vault.Context.GetContentById<T>(idString);
			        }
			        catch (ArgumentException ex)
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
