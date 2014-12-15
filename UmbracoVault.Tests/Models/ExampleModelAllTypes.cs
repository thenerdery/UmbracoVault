using UmbracoVault.Attributes;

namespace UmbracoVault.Tests.Models
{
    public class ExampleModelAllTypes
    {
        [UmbracoProperty]
        public bool Bool { get; set; }

        [UmbracoProperty]
        public byte Byte { get; set; }

        [UmbracoProperty]
        public char Char { get; set; }

        [UmbracoProperty]
        public decimal Decimal { get; set; }

        [UmbracoProperty]
        public double Double { get; set; }

        [UmbracoProperty]
        public float Float{ get; set; }

        [UmbracoProperty]
        public int[] IntArray { get; set; }

        [UmbracoProperty]
        public int Int { get; set; }

        [UmbracoProperty]
        public long Long { get; set; }

        [UmbracoProperty]
        public object Object { get; set; }

        [UmbracoProperty]
        public sbyte Sbyte { get; set; }

        [UmbracoProperty]
        public short Short { get; set; }

        [UmbracoProperty]
        public string[] StringArray { get; set; }

        [UmbracoProperty]
        public string String { get; set; }

        [UmbracoProperty]
        public uint UInt { get; set; }

        [UmbracoProperty]
        public ulong ULong { get; set; }

        [UmbracoProperty]
        public ushort UShort { get; set; }

        [UmbracoEnumProperty]
        public ExampleEnum ExampleEnum { get; set; }
    }
}
