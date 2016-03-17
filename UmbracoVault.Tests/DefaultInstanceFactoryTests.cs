using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UmbracoVault.Attributes;
using UmbracoVault.Reflection;

namespace UmbracoVault.Tests
{
    [TestClass]
    public class DefaultInstanceFactoryTests
    {
        private DefaultInstanceFactory _factory;

        [TestInitialize]
        public void InitializeTest()
        {
            _factory = new DefaultInstanceFactory();
        }

        [TestMethod]
        public void GetPropertiesToFill_ShouldReturnCorrectProperties_WithAutomap()
        {
            var properties = _factory.GetPropertiesToFill<DocumentModel>();
            Assert.AreEqual(4, properties.Count);
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "Introduction"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "Body"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "ImageUrl"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "ButtonText"));
        }

        [TestMethod]
        public void GetPropertiesToFill_ShouldReturnCorrectProperties_WithoutAutomap()
        {
            var properties = _factory.GetPropertiesToFill<NoAutoMapDocumentModel>();
            Assert.AreEqual(4, properties.Count);
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "Introduction"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "Body"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "ImageUrl"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "ButtonText"));
        }

        [UmbracoEntity(AutoMap = true)]
        // ReSharper disable once ClassNeverInstantiated.Local - OK Here, used by framework.
        private class DocumentModel
        {
            public string Introduction { get; set; }

            public string Body { get; set; }

            public string ImageUrl { get; set; }

            public virtual string ButtonText { get; set; }

            [UmbracoIgnoreProperty]
            public string Ignore { get; set; }
        }

        [UmbracoEntity(AutoMap = false)]
        // ReSharper disable once ClassNeverInstantiated.Local - Implemented by framework
        private class NoAutoMapDocumentModel
        {
            [UmbracoProperty]
            public string Introduction { get; set; }

            [UmbracoProperty]
            public string Body { get; set; }

            [UmbracoProperty]
            public string ImageUrl { get; set; }

            [UmbracoProperty]
            public virtual string ButtonText { get; set; }

            public string Ignore { get; set; }
        }
    }
}
