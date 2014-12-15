using System;
using System.Collections;
using System.Collections.Generic;

namespace UmbracoVault.Collections
{
	/// <summary>
	/// Allows for a comma-delimited string of IDs to be expanded into a type based on a passed-in iteration action function
	/// </summary>
	/// <typeparam name="T">The desired target type</typeparam>
	public class ExternalIteratorEnumerable<T> : IEnumerable<T>
	{
		internal string[] Ids;

		public Func<string, T> IterationAction = null;

		private readonly string _idValues;

		/// <summary>
		/// Allows for a comma-delimited string of IDs to be expanded into a type based on a passed-in iteration action function
		/// </summary>
		/// <param name="commaDelimitedIds">A string of comma-delimited IDs</param>
		/// <param name="iterationAction">The function that will take in a string ID and return a new type T.</param>
		public ExternalIteratorEnumerable(string commaDelimitedIds, Func<string, T> iterationAction)
		{
			this._idValues = commaDelimitedIds;
			if(!string.IsNullOrWhiteSpace(_idValues))
			{
				this.Ids = _idValues.Split(',');
			}
			IterationAction = iterationAction;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return new ExternalIteratorEnumerator<T>(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
