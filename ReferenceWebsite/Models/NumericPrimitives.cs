using UmbracoVault.Attributes;

namespace ReferenceWebsite.Models
{
    /// <summary>
    /// Vault supports most C# numeric and boolean primitives
    /// </summary>
    [UmbracoEntity(AutoMap = true)]
    public class NumericPrimitives
    {
        /// <summary>
        /// Booleans can be represented by the Umbraco true/false type.
        /// </summary>
        public bool Bool { get; set; }

        /// <summary>
        /// Bytes
        /// Supported range: 0 to 255
        /// </summary>
        public byte Byte { get; set; }

        /// <summary>
        /// Decimals
        /// Supported range: -79228162514264337593543950335 to 79228162514264337593543950335
        /// </summary>
        /// <remarks>Exceeds Umbraco numeric type</remarks>
        public decimal Decimal { get; set; }

        /// <summary>
        /// Doubles
        /// Supported range: -1.79769313486232e308 to 1.79769313486232e308
        /// </summary>
        /// <remarks>Exceeds Umbraco numeric type</remarks>
        public double Double { get; set; }

        /// <summary>
        /// Floats
        /// Supported range: -3.402823e38 to 3.402823e38
        /// </summary>
        public float Float { get; set; }

        /// <summary>
        /// Ints
        /// Supported range: -2,147,483,648 to 2,147,483,647
        /// </summary>
        public int Int { get; set; }

        /// <summary>
        /// Longs
        /// Supported range: -9,223,372,036,854,775,808 .. 9,223,372,036,854,775,807
        /// </summary>
        /// <remarks>Exceeds Umbraco numeric type</remarks>
        public long Long { get; set; }

        /// <summary>
        /// Signed Bytes
        /// Supported range: -128 to 127
        /// </summary>
        public sbyte SByte { get; set; }

        /// <summary>
        /// Shorts
        /// Supported range: -32,768 to 32,767
        /// </summary>
        public short Short { get; set; }

        /// <summary>
        /// Unsigned Ints
        /// Supported range: 0 to 4,294,967,295
        /// </summary>
        /// <remarks>Exceeds Umbraco numeric type</remarks>
        public uint UInt { get; set; }

        /// <summary>
        /// Unsigned Longs
        /// Supported range: 0 to 18,446,744,073,709,551,615
        /// </summary>
        /// <remarks>Exceeds Umbraco numeric type</remarks>
        public ulong ULong { get; set; }

        /// <summary>
        /// Unsigned Shorts
        /// Supported range: 0 to 65,535
        /// </summary>
        public ushort UShort { get; set; }
    }
}