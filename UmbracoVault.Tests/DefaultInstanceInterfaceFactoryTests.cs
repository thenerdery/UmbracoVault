using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UmbracoVault.Attributes;
using UmbracoVault.Reflection;

namespace UmbracoVault.Tests
{
    [TestClass]
    public class DefaultInstanceInterfaceFactoryTests
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

        [UmbracoEntity(AutoMap = true)]
        // ReSharper disable once ClassNeverInstantiated.Local - OK Here, used by framework.
        private class DocumentModel : IDocumentModel
        {
            public string Introduction { get; set; }

            public string Body { get; set; }

            public string ImageUrl { get; set; }

            public virtual string ButtonText { get; set; }

            [UmbracoIgnoreProperty]
            public string Ignore { get; set; }
        }

        private interface IDocumentModel
        {
            string Introduction { get; set; }

            string Body { get; set; }

            string ImageUrl { get; set; }

            string ButtonText { get; set; }

            string Ignore { get; set; }
        }
    }
}