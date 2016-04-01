using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UmbracoVault.Attributes;
using UmbracoVault.Proxy;

namespace UmbracoVault.Tests
{
    [TestClass]
    public class ProxyInstanceFactoryTests
    {
        private ProxyInstanceFactory _factory;

        [TestInitialize]
        public void InitializeTest()
        {
            _factory = new ProxyInstanceFactory();
        }

        [TestMethod]
        public void GetPropertiesToFill_ShouldReturnCorrectProperties_WithAutomap()
        {
            var properties = _factory.GetPropertiesToFill<DocumentModel>();
            Assert.AreEqual(3, properties.Count);
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "Introduction"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "Body"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "ImageUrl"));
        }

        [TestMethod]
        public void GetPropertiesToFill_ShouldReturnCorrectProperties_WithoutAutomap()
        {
            var properties = _factory.GetPropertiesToFill<NoAutoMapDocumentModel>();
            Assert.AreEqual(3, properties.Count);
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "Introduction"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "Body"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "ImageUrl"));
        }

        [UmbracoEntity(AutoMap = true)]
        // ReSharper disable once ClassNeverInstantiated.Local - Implemented by Framework
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
        // ReSharper disable once ClassNeverInstantiated.Local - Implemented by Framework
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

        /////////////////////////////////////////////////////////////////////////////////////////
        // Inheritence Tests
        /////////////////////////////////////////////////////////////////////////////////////////

        //Inner Class Automap
        [UmbracoEntity(AutoMap = true)]
        public class InnerClassWithAutoMapExtendsBaseClassWithAutoMap : BaseClassWithAutoMap
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

        [TestMethod]
        public void GetPropertiesToFill_For_InnerClassWithAutoMapExtendsBaseClassWithAutoMap_ShouldReturnCorrectProperties_WithAutomap()
        {
            var properties = _factory.GetPropertiesToFill<InnerClassWithAutoMapExtendsBaseClassWithAutoMap>();
            Assert.AreEqual(4, properties.Count);
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "Introduction"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "Body"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "ImageUrl"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "BaseProperty"));
        }

        [UmbracoEntity(AutoMap = true)]
        public class InnerClassWithAutomapExtendsBaseClass : BaseClass
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

        [TestMethod]
        public void GetPropertiesToFill_For_InnerClassWithAutomapExtendsBaseClass_ShouldReturnCorrectProperties_WithAutomap()
        {
            var properties = _factory.GetPropertiesToFill<InnerClassWithAutomapExtendsBaseClass>();
            Assert.AreEqual(5, properties.Count);
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "Introduction"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "Body"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "ImageUrl"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "BaseProperty"));
            // Currently Intentional, an AutoMap on the inner class will override parent classes, and if ignore properties are not applied it will be mapped.
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "BaseIgnore"));
        }

        [UmbracoEntity(AutoMap = true)]
        public class InnerClassWithAutoMapExtendsBaseClassNoAttribute : BaseClassNoAttribute
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

        [TestMethod]
        public void GetPropertiesToFill_For_InnerClassWithAutoMapExtendsBaseClassNoAttribute_ShouldReturnCorrectProperties_WithAutomap()
        {
            var properties = _factory.GetPropertiesToFill<InnerClassWithAutoMapExtendsBaseClassNoAttribute>();
            Assert.AreEqual(5, properties.Count);
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "Introduction"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "Body"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "ImageUrl"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "BaseProperty"));
            // Currently Intentional, an AutoMap on the inner class will override parent classes, and if ignore properties are not applied it will be mapped.
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "BaseIgnore"));
        }

        //Inner Class Normal

        [UmbracoEntity]
        public class InnerClassExtendsBaseClassAutoMap : BaseClassWithAutoMap
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

            [UmbracoIgnoreProperty]
            public string Ignore3 { get; set; }
        }

        [TestMethod]
        public void GetPropertiesToFill_For_InnerClassExtendsBaseClassAutoMap_ShouldReturnCorrectProperties_WithoutAutomap()
        {
            var properties = _factory.GetPropertiesToFill<InnerClassExtendsBaseClassAutoMap>();
            Assert.AreEqual(3, properties.Count);
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "Introduction"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "Body"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "ImageUrl"));
        }

        [UmbracoEntity] // AutoMap = False
        public class InnerClassExtendsBaseClass : BaseClass
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

            [UmbracoIgnoreProperty]
            public string Ignore3 { get; set; }
        }

        [TestMethod]
        public void GetPropertiesToFill_For_InnerClassExtendsBaseClass_ShouldReturnCorrectProperties_WithoutAutomap()
        {
            var properties = _factory.GetPropertiesToFill<InnerClassExtendsBaseClass>();
            Assert.AreEqual(4, properties.Count);
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "Introduction"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "Body"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "ImageUrl"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "BaseProperty"));
        }

        [UmbracoEntity] // AutoMap = False
        public class InnerClassExtendsBaseClassNoAttribute : BaseClassNoAttribute
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

            [UmbracoIgnoreProperty]
            public string Ignore3 { get; set; }
        }

        [TestMethod]
        public void GetPropertiesToFill_For_InnerClassExtendsBaseClassNoAttribute_ShouldReturnCorrectProperties_WithoutAutomap()
        {
            var properties = _factory.GetPropertiesToFill<InnerClassExtendsBaseClassNoAttribute>();
            Assert.AreEqual(3, properties.Count);
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "Introduction"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "Body"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "ImageUrl"));
        }

        //Inner Class No Attribute

        public class InnerClassNoAttributeExtendsBaseClassAutoMap : BaseClassWithAutoMap
        {
            public string Introduction { get; set; }

            public string Body { get; set; }

            public string ImageUrl { get; set; }

            public virtual string ButtonText { get; set; }

            public string Ignore1 { get; set; }

            public virtual string Ignore2 { get; set; }
        }

        // TODO: Change this behavior later on, probably shouldn't work like this, but retain for now
        // for backewards compatibility
        [TestMethod]
        public void GetPropertiesToFill_For_InnerClassNoAttributeExtendsBaseClassAutoMap_ShouldReturnCorrectProperties_WithoutAutomap()
        {
            var properties = _factory.GetPropertiesToFill<InnerClassNoAttributeExtendsBaseClassAutoMap>();
            Assert.AreEqual(5, properties.Count);
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "Introduction"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "Body"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "ImageUrl"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "BaseProperty"));
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "Ignore1"));
        }

        public class InnerClassNoAttributeExtendsBaseClass : BaseClass
        {
            public string Introduction { get; set; }

            public string Body { get; set; }

            public string ImageUrl { get; set; }

            public virtual string ButtonText { get; set; }

            public string Ignore1 { get; set; }

            public virtual string Ignore2 { get; set; }
        }

        // TODO: Change this behavior later on, probably shouldn't work like this, but retain for now
        // for backewards compatibility
        [TestMethod]
        public void GetPropertiesToFill_For_InnerClassNoAttributeExtendsBaseClass_ShouldReturnCorrectProperties_WithoutAutomap()
        {
            var properties = _factory.GetPropertiesToFill<InnerClassNoAttributeExtendsBaseClass>();
            Assert.AreEqual(1, properties.Count);
            Assert.IsNotNull(properties.FirstOrDefault(p => p.Name == "BaseProperty"));
        }

        public class InnerClassNoAttributeExtendsBaseClassNoAttribute : BaseClassNoAttribute
        {
            public string Introduction { get; set; }

            public string Body { get; set; }

            public string ImageUrl { get; set; }

            public virtual string ButtonText { get; set; }

            public string Ignore1 { get; set; }

            public virtual string Ignore2 { get; set; }

        }

        // TODO: Change this behavior later on, probably shouldn't work like this, but retain for now
        // for backewards compatibility
        [TestMethod]
        public void GetPropertiesToFill_For_InnerClassNoAttributeExtendsBaseClassNoAttribute_ShouldReturnCorrectProperties_WithoutAutomap()
        {
            var properties = _factory.GetPropertiesToFill<InnerClassNoAttributeExtendsBaseClassNoAttribute>();
            Assert.AreEqual(0, properties.Count);
        }

        //Base Classes

        [UmbracoEntity(AutoMap = true)]
        public class BaseClassWithAutoMap
        {
            public string BaseProperty { get; set; }

            [UmbracoIgnoreProperty]
            public string BaseIgnore { get; set; }

            public virtual string BaseVirtual { get; set; }
        }

        [UmbracoEntity] // AutoMap = False
        public class BaseClass
        {
            [UmbracoProperty]
            public string BaseProperty { get; set; }

            public string BaseIgnore { get; set; }

            [UmbracoProperty]
            public virtual string BaseVirtual { get; set; }
        }

        

        public class BaseClassNoAttribute
        {
            public string BaseProperty { get; set; }

            public string BaseIgnore { get; set; }

            public virtual string BaseVirtual { get; set; }
        }
    }
}
