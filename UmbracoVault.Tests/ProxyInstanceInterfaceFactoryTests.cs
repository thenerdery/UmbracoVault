using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UmbracoVault.Attributes;
using UmbracoVault.Proxy;

namespace UmbracoVault.Tests
{
    [TestClass]
    public class ProxyInstanceInterfaceFactoryTests
    {
        private ProxyInstanceFactory _factory;

        [TestInitialize]
        public void InitializeTest()
        {
            _factory = new ProxyInstanceFactory();
        }

        [TestMethod]
        public void GetPropertiesToFillOnImpliedInterfaceImplementation_ShouldReturnCorrectProperties_WithAutomap()
        {
            var properties = _factory.GetPropertiesToFill<ImpliedInterfaceDocumentModel>();
            Assert.AreEqual(3, properties.Count);
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "Introduction"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "Body"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "ImageUrl"));
        }

        [UmbracoEntity(AutoMap = true)]
        // ReSharper disable once ClassNeverInstantiated.Local - OK Here, used by framework.
        private class ImpliedInterfaceDocumentModel : IDocumentModel
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

        [TestMethod]
        public void GetPropertiesToFillOnExplicitInterfaceImplementation_ShouldReturnCorrectProperties_WithAutomap()
        {
            var properties = _factory.GetPropertiesToFill<ExplicitInterfaceDocumentModel>();
            Assert.AreEqual(4, properties.Count);

            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "UmbracoVault.Tests.ProxyInstanceInterfaceFactoryTests.IDocumentModel.Introduction"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "UmbracoVault.Tests.ProxyInstanceInterfaceFactoryTests.IDocumentModel.Body"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "UmbracoVault.Tests.ProxyInstanceInterfaceFactoryTests.IDocumentModel.ImageUrl"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "UmbracoVault.Tests.ProxyInstanceInterfaceFactoryTests.IDocumentModel.ButtonText"));
        }

        [UmbracoEntity(AutoMap = true)]
        // ReSharper disable once ClassNeverInstantiated.Local - OK Here, used by framework.
        private class ExplicitInterfaceDocumentModel : IDocumentModel
        {
            string IDocumentModel.Introduction { get; set; }
            string IDocumentModel.Body { get; set; }
            string IDocumentModel.ImageUrl { get; set; }
            string IDocumentModel.ButtonText { get; set; }

            [UmbracoIgnoreProperty]
            string IDocumentModel.Ignore { get; set; }
        }
    }
}