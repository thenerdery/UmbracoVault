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
		internal readonly string[] Ids;

		public readonly Func<string, T> IterationAction;

		/// <summary>
		/// Allows for a comma-delimited string of IDs to be expanded into a type based on a passed-in iteration action function
		/// </summary>
		/// <param name="commaDelimitedIds">A string of comma-delimited IDs</param>
		/// <param name="iterationAction">The function that will take in a string ID and return a new type T.</param>
		public ExternalIteratorEnumerable(string commaDelimitedIds, Func<string, T> iterationAction)
		{
			if(!string.IsNullOrWhiteSpace(commaDelimitedIds))
			{
				Ids = commaDelimitedIds.Split(',');
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
