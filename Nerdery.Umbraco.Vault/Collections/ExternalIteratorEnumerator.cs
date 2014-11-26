using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nerdery.Umbraco.Vault.Collections
{
	public class ExternalIteratorEnumerator<T> : IEnumerator<T>
	{
		private readonly ExternalIteratorEnumerable<T> _source;
		private IList<T> _items = new List<T>();

		private int _index;

		public ExternalIteratorEnumerator(ExternalIteratorEnumerable<T> externalIteratorEnumerable)
		{
			_source = externalIteratorEnumerable;
			_index = -1;
		}

		public void Dispose()
		{
			_items = null;
		}

		/// <summary>
		/// Advances the internal index
		/// </summary>
		/// <returns>Returns false if we cannot advance the internal index (you have iterated through all the items)</returns>
		public bool MoveNext()
		{
			if (_source.Ids == null) return false;
			if(_source.Ids.Count() > _index + 1)
			{
				_index++;
				return true;
			}

			return false;
		}

		public void Reset()
		{
			// Reset to -1 to allow the movenext to return the first element (this is standard Enumerable behavior)
			_index = -1;
		}

		/// <summary>
		/// Builds the current item using the iteration action provided to the source ExternalIteratorEnumerable object.
		/// If the current item has already been built, it is simply returned from memory.
		/// </summary>
		public T Current
		{
			get
			{
				// If we've already loaded an item at or beyond that index, then just return it. 
				if (_items.Count() > _index + 1)
				{
					return _items[_index];
				}

				// Otherwise, we need to create the item, push it into items, then we can return it.
				var idString = _source.Ids[_index];
				
				if (_source != null)
				{
					T item = _source.IterationAction(idString);
					_items.Insert(_index, item);
					return item;
				}
				return default(T);
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return Current;
			}
		}
	}
}
