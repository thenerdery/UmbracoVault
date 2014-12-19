using Microsoft.VisualStudio.TestTools.UnitTesting;
using UmbracoVault.Tests.Models;
using UmbracoVault.TypeHandlers;
using UmbracoVault.TypeHandlers.Primitives;

namespace UmbracoVault.Tests
{
    [TestClass]
    public class TypeHandlerFactoryTests
    {
        [TestMethod]
        public void TypeHandler_Factory_Registers_String_Type()
        {
            var factory = TypeHandlerFactory.Instance;
            var stringHandlerType = factory.GetHandlerForType(typeof (string));

            Assert.AreEqual(typeof(StringTypeHandler), stringHandlerType.GetType());
        }

        [TestMethod]
        public void TypeHandler_Factory_Can_Accept_External_Registration()
        {
            var factory = TypeHandlerFactory.Instance;
            
            factory.RegisterTypeHandler<CustomTypeHandler>();
            var customHandlerType = factory.GetHandlerForType(typeof(ExampleModelAllTypes));


            Assert.AreEqual(typeof(CustomTypeHandler), customHandlerType.GetType());
        }

        [TestMethod]
        public void Duplicate_Type_Handlers_Are_Handled_Gracefully()
        {
            var factory = TypeHandlerFactory.Instance;

            factory.RegisterTypeHandler<CustomTypeHandler>();
            factory.RegisterTypeHandler<CustomTypeHandler>(); //duplicate
            var customHandlerType = factory.GetHandlerForType(typeof(ExampleModelAllTypes));


            Assert.AreEqual(typeof(CustomTypeHandler), customHandlerType.GetType());
        }

        [TestMethod]
        public void Vault_Will_Auto_Register_Types_From_External_Assemblies()
        {
            var factory = TypeHandlerFactory.Instance;

            //See Properties/AssemblyInfo.cs for [assembly:ContainsUmbracoVaultTypeHandlers] attribute
            //See Models/AutoRegisteredTypeHandler.cs for type expected for registration
            var autoRegisteredTypeHandler = factory.GetHandlerForType(typeof(AutoRegisteredType));

            Assert.IsNotNull(autoRegisteredTypeHandler);
        }

        [TestMethod]
        public void Vault_Will_Not_Auto_Register_Type_Handlers_Marked_As_Ignored()
        {
            var factory = TypeHandlerFactory.Instance;

            //See Models/IgnoredTypeHandler.cs
            var ignoredTypeHandler = factory.GetHandlerForType(typeof(IgnoredType));

            Assert.IsNull(ignoredTypeHandler);
        }
    }
}
