using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UmbracoVault.Collections;
using UmbracoVault.Tests.Models;

namespace UmbracoVault.Tests
{
    [TestClass]
    public class TypeAliasSetTests
    {
        [TestMethod]
        public void GetAliasesForUmbracoEntityType_ShouldBeDistinct()
        {
            var aliases = TypeAliasSet.GetAliasesForUmbracoEntityType(typeof(SecondLevelRecursiveModelWithAliases)).OrderBy(x => x);
            Assert.IsTrue(aliases.SequenceEqual(aliases.Distinct()));
        }

        [TestMethod]
        public void GetAliasesForUmbracoEntityType_ShouldUseCamelCaseClassNameWhenAliasIsNotSpecified()
        {
            var modelType = typeof(RecursiveModelWithAliases);
            var aliases = TypeAliasSet.GetAliasesForUmbracoEntityType(modelType);

            var expectedAlias = new string(modelType.Name.Select((c, i) => i == 0 ? char.ToLower(c) : c).ToArray());

            Assert.AreEqual(expectedAlias, aliases.Single());
        }

        [TestMethod]
        public void GetAliasesForUmbracoEntityType_ShouldRemoveViewModelFromClassNames()
        {
            var modelType = typeof(ExampleRecursiveAttributesViewModel);
            var aliases = TypeAliasSet.GetAliasesForUmbracoEntityType(modelType);

            var expectedAlias = new string(modelType.Name.Replace("ViewModel", "").Select((c, i) => i == 0 ? char.ToLower(c) : c).ToArray());

            Assert.AreEqual(expectedAlias, aliases.Single());
        }

        [TestMethod]
        public void GetAliasesForUmbracoEntityType_ShouldUseCamelCaseComparisonForContains()
        {
            var modelType = typeof(ExampleRecursiveAttributesViewModel);
            var aliases = TypeAliasSet.GetAliasesForUmbracoEntityType(modelType);

            var testPropertyName = modelType.Name.Replace("ViewModel", "");

            Assert.IsTrue(aliases.Contains(testPropertyName));
        }

        [TestMethod]
        public void GetAliasesForUmbracoEntityType_ShouldIncludeAllRecursiveAliases()
        {
            var firstLevelAliases = TypeAliasSet.GetAliasesForUmbracoEntityType(typeof(RecursiveModelWithAliases));
            var secondLevelAliases = TypeAliasSet.GetAliasesForUmbracoEntityType(typeof(SecondLevelRecursiveModelWithAliases));

            Assert.AreEqual(2, secondLevelAliases.Count);
            Assert.IsTrue(secondLevelAliases.IsSupersetOf(firstLevelAliases));
        }
    }
}
