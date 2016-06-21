using System;
using System.Collections.Generic;

namespace UmbracoVault
{
    public static class Vault
    {
        private static IUmbracoContext _instance = new UmbracoWebContext();
        private static readonly List<string> _defaultControllerNamespaces = new List<string>();
        private static bool _isLocked;

        // TODO: add API documentation
        public static void RegisterViewModelNamespace(string ns, string assemblyName)
        {
            if (_isLocked)
            {
                throw new InvalidOperationException("You cannot make changes to Umbraco Vault configuration after application startup has occurred.");
            }
            var t = $"{ns}.{{0}}ViewModel,{assemblyName}";
            _defaultControllerNamespaces.Add(t);
        }

        internal static List<string> GetRegisteredNamespaces()
        {
            return _defaultControllerNamespaces;
        }

        internal static void LockConfiguration()
        {
            _isLocked = true;
        }

        /// <summary>
        /// Retrieves an Umbraco Context to be used to generate Vault objects
        /// </summary>
        public static IUmbracoContext Context => _instance;

        public static void SetOverrideContext(IUmbracoContext context)
        {
            _instance = context;
        }
    }
}
