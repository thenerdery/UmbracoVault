using System;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UmbracoVault.Tests.Models;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace UmbracoVault.Tests
{
    [TestClass]
    public class UmbracoContextTests
    {
        [TestClass]
        public class FillClassPropertiesTests
        {
            public ExampleModelAllTypes Source { get; set; }
            public ExampleModelAllTypes Destination { get; set; }
            public UmbracoWebContext Context { get; set; }

            [TestInitialize]
            public void Initialize()
            {
                Destination = new ExampleModelAllTypes();
                Context = new UmbracoWebContext();

                var fixture = new Fixture().Customize(new AutoMoqCustomization());
                Source = fixture.Freeze<ExampleModelAllTypes>();

                Source.IntArray = new [] {1,2,3};
                Source.StringArray = new[] {"100", "200", "300"};
                Source.Object = new { Name = "TestObject" };
                Source.ExampleEnum = ExampleEnum.Harry;
                Source.TypeHandledString = "       this string needs to be trimmed        ";

                var specialCases = new NameValueCollection
                {
                    {"IntArray", "1,2,3"},
                    {"StringArray", "100,200,300"},
                    {"ExampleEnum", "Harry"}
                };

                Context.FillClassProperties(Destination, (alias, recursive) =>
                {
                    var property = typeof (ExampleModelAllTypes).GetProperties()
                                                                .FirstOrDefault(p => string.Equals(p.Name, alias, StringComparison.InvariantCultureIgnoreCase));

                    if (property != null)
                    {
                        var hasSpecialValue = specialCases.AllKeys.Contains(property.Name);

                        var value = hasSpecialValue
                            // ReSharper disable once AssignNullToNotNullAttribute
                            ? specialCases.GetValues(property.Name).First()
                            : property.GetValue(Source).ToString();

                        return value;

                    }

                    return null;
                });
            }

            [TestMethod]
            public void Bool_IsSet()
            {
                Assert.AreNotEqual(Destination.Bool, default(bool));
                Assert.AreEqual(Source.Bool, Destination.Bool);
            }

            [TestMethod]
            public void Byte_IsSet()
            {
                Assert.AreNotEqual(Destination.Byte, default(byte));
                Assert.AreEqual(Source.Byte, Destination.Byte);
            }

            [TestMethod]
            public void Char_IsSet()
            {
                Assert.AreNotEqual(Destination.Char, default(char));
                Assert.AreEqual(Source.Char, Destination.Char);
            }

            [TestMethod]
            public void Decimal_IsSet()
            {
                Assert.AreNotEqual(Destination.Decimal, default(decimal));
                Assert.AreEqual(Source.Decimal, Destination.Decimal);
            }

            [TestMethod]
            public void Double_IsSet()
            {
                Assert.AreNotEqual(Destination.Double, default(double));
                Assert.AreEqual(Source.Double, Destination.Double);
            }

            [TestMethod]
            public void Float_IsSet()
            {
                Assert.AreNotEqual(Destination.Float, default(float));
                Assert.AreEqual(Source.Float, Destination.Float);
            }

            [TestMethod]
            public void IntArray_IsSet()
            {
                Assert.AreNotEqual(Destination.IntArray, default(int[]));
                Assert.AreEqual(Source.IntArray.Length, Source.IntArray.Length);

                for (int i = 0; i < Source.IntArray.Length; i++)
                {
                    Assert.AreEqual(Source.IntArray[i], Destination.IntArray[i]);
                }
            }

            [TestMethod]
            public void Int_IsSet()
            {
                Assert.AreNotEqual(Destination.Int, default(int));
                Assert.AreEqual(Source.Int, Destination.Int);
            }

            [TestMethod]
            public void Long_IsSet()
            {
                Assert.AreNotEqual(Destination.Long, default(long));
                Assert.AreEqual(Source.Long, Destination.Long);
            }

            [TestMethod]
            public void Object_IsSet()
            {
                // TODO: This test appears flawed.  The Destination object property has the string representation of the source object
                Assert.AreNotEqual(Destination.Object, default(object));
                Assert.AreEqual(Source.Object.ToString(), Destination.Object);
            }

            [TestMethod]
            public void Sbyte_IsSet()
            {
                Assert.AreNotEqual(Destination.Sbyte, default(sbyte));
                Assert.AreEqual(Source.Sbyte, Destination.Sbyte);
            }

            [TestMethod]
            public void Short_IsSet()
            {
                Assert.AreNotEqual(Destination.Short, default(short));
                Assert.AreEqual(Source.Short, Destination.Short);
            }

            [TestMethod]
            public void StringArray_IsSet()
            {
                Assert.AreNotEqual(Destination.StringArray, default(string[]));
                Assert.AreEqual(Source.StringArray.Length, Source.StringArray.Length);

                for (int i = 0; i < Source.StringArray.Length; i++)
                {
                    Assert.AreEqual(Source.StringArray[i], Destination.StringArray[i]);
                }
            }

            [TestMethod]
            public void String_IsSet()
            {
                Assert.AreNotEqual(Destination.String, default(string));
                Assert.AreEqual(Source.String, Destination.String);
            }

            [TestMethod]
            public void TypeHandler_ShouldExecute_EvenWhenTypesAlreadyMatch()
            {
                Assert.AreEqual(Source.TypeHandledString.Trim(), Destination.TypeHandledString);
            }

            [TestMethod]
            public void UInt_IsSet()
            {
                Assert.AreNotEqual(Destination.UInt, default(uint));
                Assert.AreEqual(Source.UInt, Destination.UInt);
            }

            [TestMethod]
            public void ULong_IsSet()
            {
                Assert.AreNotEqual(Destination.ULong, default(ulong));
                Assert.AreEqual(Source.ULong, Destination.ULong);
            }

            [TestMethod]
            public void UShort_IsSet()
            {
                Assert.AreNotEqual(Destination.UShort, default(ushort));
                Assert.AreEqual(Source.UShort, Destination.UShort);
            }

            [TestMethod]
            public void Enum_IsSet()
            {
                Assert.AreEqual(Source.ExampleEnum, Destination.ExampleEnum);
            }

            [TestMethod]
            public void GetUmbracoEntityAliasesFromType_ShouldBeDistinct()
            {
                var aliases = UmbracoWebContext.GetUmbracoEntityAliasesFromType(typeof(ExampleRecursiveAttributesViewModel)).OrderBy(x => x);
                Assert.IsTrue(aliases.SequenceEqual(aliases.Distinct()));
            }
        }
    }
}
