using System.Web.Razor.Generator;
using Nerdery.Umbraco.Vault.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uComponents.DataTypes.UrlPicker.Dto;
using umbraco.presentation.webservices;
using Umbraco.Web.Models;

namespace ReferenceWebsite.Models
{
    #region Primitive Examples

    /// <summary>
    /// Vault supports most C# numeric and boolean primitives
    /// </summary>
    [UmbracoEntity(AutoMap = true)]
    public class NumericPrimitives
    {
        /// <summary>
        /// Booleans can be represented by the Umbraco true/false type
        /// </summary>
        public bool Bool { get; set; }

        /// <summary>
        /// Bytes can be represented with a numeric.
        /// Validation regular expression: 
        /// </summary>
        public byte Byte { get; set; }

        /// <summary>
        /// Decimals can be represented with a numeric.
        /// Validation regular expression: 
        /// </summary>
        public decimal Decimal { get; set; }

        /// <summary>
        /// Doubles can be represented with a numeric.
        /// Validation regular expression: 
        /// </summary>
        public double Double { get; set; }

        /// <summary>
        /// Floats can be represented with a numeric.
        /// Validation regular expression: 
        /// </summary>
        public float Float { get; set; }

        /// <summary>
        /// Ints can be represented with a numeric.
        /// Validation regular expression: 
        /// </summary>
        public int Int { get; set; }

        /// <summary>
        /// Longs exceed the Umbraco integer size, so should use textstring
        /// Validation regular expression: 
        /// </summary>
        public long Long { get; set; }

        /// <summary>
        /// SBytes can be represented with a numeric.
        /// Validation regular expression: 
        /// </summary>
        public sbyte SByte { get; set; }

        /// <summary>
        /// Shorts can be represented with a numeric.
        /// Validation regular expression: 
        /// </summary>
        public short Short { get; set; }

        /// <summary>
        /// Unsigned Ints exceed the Umbraco integer size, so should use textstring
        /// Validation regular expression: 
        /// </summary>
        public uint UInt { get; set; }

        /// <summary>
        /// Unsigned Longs exceed the Umbraco integer size, so should use textstring
        /// Validation regular expression: 
        /// </summary>
        public ulong ULong { get; set; }

        /// <summary>
        /// Unsigned Shorts can be represented with a numeric.
        /// Validation regular expression: 
        /// </summary>
        public ushort UShort { get; set; }
    }

    [UmbracoEntity(AutoMap = true)]
    public class TextPrimitives
    {
        /// <summary>
        /// A char can hold a single character. Represent in Umbraco with a textstring
        /// Validation regular expression: 
        /// </summary>
        public char Char { get; set; }

        /// <summary>
        /// A string can hold many characters. Represent in Umbraco with a textstring.
        /// </summary>
        public string String { get; set; }

        /// <summary>
        /// Rich text is a string with macros applied. Represent in Umbraco with a richtext editor.
        /// </summary>
        [UmbracoRichTextProperty]
        public string RichText { get; set; }
    }

    #endregion

    #region Array Examples

    [UmbracoEntity(AutoMap = true)]
    public class Arrays
    {
        /// <summary>
        /// Raw integer arrays are supported using a textstring
        /// </summary>
        public int[] IntArray { get; set; }

        /// <summary>
        /// Raw string arrays are supported using a textstring
        /// </summary>
        public string[] StringArray { get; set; }
    }

    #endregion

    #region Object Examples

    [UmbracoEntity(AutoMap = true)]
    public class Objects
    {
        public object Object { get; set; }
    }

    [UmbracoMediaEntity(AutoMap = true)]
    public class Image
    {
        [UmbracoProperty(Alias = "umbracoFile")]
        public string Url { get; set; }
        public string Alt { get; set; }
    }

    [UmbracoEntity(AutoMap=true)]
    public class Location
    {
        public string Name { get; set; }
    }

    [UmbracoEntity(AutoMap = true, Alias = "person" )]
    public class StaffMember
    {
        public string Name { get; set; }
        public Location PrimaryLocation { get; set; }
    }

    #endregion

    #region uComponents Examples

    [UmbracoEntity(AutoMap = true)]
    public class UComponentsUrlModel
    {
        public UrlPickerState UrlState { get; set; }
    }

    [UmbracoEntity(AutoMap = true)]
    public class UComponentsDataTypeGridItem
    {
        public string Name { get; set; }
        public string Date { get; set; }
    }

    [UmbracoEntity(AutoMap = true)]
    public class UComponentsDataTypeGridModel
    {
        public List<UComponentsDataTypeGridItem> Items { get; set; }
    }

    #endregion

    #region Enums

    public enum Month
    {
        January = 1,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }

    [UmbracoEntity(AutoMap=true)]
    public class DateModel
    {
        public int Year { get; set; }
        [UmbracoEnumProperty]
        public Month Month { get; set; }
        public int Day { get; set; }
    }

    #endregion
}