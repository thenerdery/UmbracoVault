using Nerdery.Umbraco.Vault.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uComponents.DataTypes.UrlPicker.Dto;

namespace ReferenceWebsite.Models
{
    #region Primitive Examples

    [UmbracoEntity(AutoMap = true)]
    public class NumericPrimitives
    {
        public bool Bool { get; set; }
        public byte Byte { get; set; }
        public decimal Decimal { get; set; }
        public double Double { get; set; }
        public float Float { get; set; }
        public int Int { get; set; }
        public long Long { get; set; }
        public sbyte SByte { get; set; }
        public short Short { get; set; }
        public uint UInt { get; set; }
        public ulong ULong { get; set; }
        public ushort UShort { get; set; }
    }

    [UmbracoEntity(AutoMap = true)]
    public class TextPrimitives
    {
        public char Char { get; set; }
        public string String { get; set; }
    }

    #endregion

    #region Array Examples

    [UmbracoEntity(AutoMap = true)]
    public class Arrays
    {
        public int[] IntArray { get; set; }
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

#endregion
}