using System;

namespace Nerdery.Umbraco.Vault.TypeHandlers.Primitives
{
    /// <summary>
	/// This primitive implementation assumes the string can be split on a comma to generate the array
	/// If this is not the case, a different, custom handler should be utilized.
	/// </summary>
	public class StringArrayTypeHandler : ITypeHandler
	{
		private object Get(string stringValue)
		{
			return stringValue.Split(',');
		}

        public object GetAsType<T>(object input)
		{
			return Get(input.ToString());
		}

		public Type TypeSupported
		{
			get
			{
				return typeof(string[]);
			}
		}
	}
}
