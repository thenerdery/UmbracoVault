using System;

namespace UmbracoVault.Attributes
{
    /// <summary>
	/// Denotes that an entity represents an Umbraco document type.
	/// Also used to configure certain class-level options, such as AutoMap
	/// In the case of multiple [UmbracoEntity] mappings for a class, the first attribute is used for class-wide options
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class UmbracoEntityAttribute : Attribute
	{
		public UmbracoEntityAttribute()
		{
		}

		/// <summary>
		/// Only needed if the name of the Entity is not the same as the umbraco Doc Type alias
		/// </summary>
		/// <param name="alias">The umbraco Doc Type alias</param>
		public UmbracoEntityAttribute(string alias)
		{
			Alias = alias;
		}

        /// <summary>
        /// When false (default), you must manually opt in all properties to be mapped with the [UmbracoProperty] attribute.
        /// When true, you can omit the [UmbracoProperty] attribute and all properties will be mapped that are not decorated with [UmbracoIgnoreProperty].
        /// </summary>
        public bool AutoMap { get; set; }

		/// <summary>
		/// The Umbraco Doc Type alias, if it is different from the name of the entity
		/// </summary>
		public string Alias { get; set; }

        /// <summary>
        /// Allows a class-level attribute to override the type handler
        /// Typehandler must implement ITypeHandler
        /// </summary>
        public virtual Type TypeHandlerOverride { get; set; }
	}
}
