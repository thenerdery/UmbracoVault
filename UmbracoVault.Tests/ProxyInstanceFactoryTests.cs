using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UmbracoVault.Attributes;
using UmbracoVault.Proxy.Concrete;

namespace UmbracoVault.Tests
{
    [TestClass]
    public class ProxyInstanceFactoryTests
    {
        private ProxyInstanceFactory factory;

        [TestInitialize]
        public void InitializeTest()
        {
            factory = new ProxyInstanceFactory();
        }

        [TestMethod]
        public void GetPropertiesToFill_ShouldReturnCorrectProperties_WithAutomap()
        {
            var properties = factory.GetPropertiesToFill<DocumentModel>();
            Assert.AreEqual(3, properties.Count);
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "Introduction"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "Body"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "ImageUrl"));
        }

        [TestMethod]
        public void GetPropertiesToFill_ShouldReturnCorrectProperties_WithoutAutomap()
        {
            var properties = factory.GetPropertiesToFill<NoAutoMapDocumentModel>();
            Assert.AreEqual(3, properties.Count);
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "Introduction"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "Body"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "ImageUrl"));
        }

        [UmbracoEntity(AutoMap = true)]
        private class DocumentModel
        {
            public string Introduction { get; set; }

            public string Body { get; set; }

            public string ImageUrl { get; set; }

            public virtual string ButtonText { get; set; }

            [UmbracoIgnoreProperty]
            public string Ignore1 { get; set; }

            [UmbracoIgnoreProperty]
            public virtual string Ignore2 { get; set; }
        }

        [UmbracoEntity(AutoMap = false)]
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

            public string Ignore1 { get; set; }

            public virtual string Ignore2 { get; set; }
        }
    }
}
