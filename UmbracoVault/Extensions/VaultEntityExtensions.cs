using System;
using System.Collections.Generic;
using System.Linq;
using UmbracoVault.Attributes;
using UmbracoVault.Models;

namespace UmbracoVault.Extensions
{
    internal static class VaultEntityExtensions
    {
        private static List<VaultEntity> _entities;
        private static readonly object Padlock = new object();

        internal static List<VaultEntity> GetValutEntities()
        {
            if (_entities == null)
            {
                lock (Padlock)
                {
                    if (_entities == null)
                    {
                        _entities = LoadVaultEntities();
                    }
                }
            }

            return _entities;
        }

        private static List<VaultEntity> LoadVaultEntities()
        {
            var output = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(
                        x =>
                        {
                            Type[] types;
                            try
                            {
                                types = x.GetTypes();
                            }
                            // TODO: We may need to create a fluent registration system for these viewmodels.
                            catch (Exception) // TODO: Generic exception says 'whaa!?'
                            {
                                return new Type[] { };
                            }
                            return types;
                        })
                        .Select(x =>
                        {
                            try
                            {
                                return new VaultEntity
                                {
                                    MetaData = ((UmbracoEntityAttribute)Attribute.GetCustomAttribute(x, typeof(UmbracoEntityAttribute), false)),
                                    Type = x

                                };
                            }
                            catch (TypeLoadException) // Ignore type loads
                            {
                                return new VaultEntity();
                            }
                        })
                        .Where(x => x.MetaData != null)
                        .ToList();

            return output;
        }
    }
}
