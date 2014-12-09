using System;
using System.Collections.Generic;

namespace Nerdery.Umbraco.Vault
{
    public static class Vault
    {
        private static readonly List<string> _defaultControllerNamespaces = new List<string>();
        private static bool _isLocked = false;

        // TODO: add API documentation
        public static void RegisterViewModelNamespace(string ns, string assemblyName)
        {
            if (_isLocked)
            {
                throw new InvalidOperationException("You cannot make changes to Umbraco Vault configuration after application startup has occurred.");
            }
            var t = string.Format("{0}.{{0}}ViewModel,{1}", ns, assemblyName);
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
        public static IUmbracoContext Context
        {
            get { return new UmbracoWebContext(); }
        }
    }
}
