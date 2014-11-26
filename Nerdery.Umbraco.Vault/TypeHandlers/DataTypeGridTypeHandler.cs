using System;
using System.Collections.Generic;
using System.Linq;

using Nerdery.Umbraco.Vault.Attributes;
using Nerdery.Umbraco.Vault.Exceptions;
using Nerdery.Umbraco.Vault.Extensions;

using uComponents.DataTypes.DataTypeGrid.Model;

namespace Nerdery.Umbraco.Vault.TypeHandlers
{
    [IgnoreAutoLoad]
    public class DataTypeGridTypeHandler : ITypeHandler
    {
        public object GetAsType<T>(object input)
        {
            var result = new List<T>();
            var rows = input as GridRowCollection;
            if (rows != null)
            {
                foreach (var row in rows)
                {
                    var item = typeof(T).CreateWithNoParams<T>();

                    if (item == null)
                    {
                        throw new ConstructorUnavailableException(typeof(T));
                    }

                    var currentRow = row;
                    Vault.InternalContext.FillClassProperties(item, (alias, recursive) =>
                    {
                        // recursive is currently ignored in this case

                        string value = null;
                        if (!string.IsNullOrWhiteSpace(alias) && currentRow != null)
                        {
                            var cell = currentRow.FirstOrDefault(c => c.Alias == alias);
                            if (cell != null)
                            {
                                value = string.IsNullOrWhiteSpace(cell.Value) ? null : cell.Value;
                            }
                        }

                        return value;
                    });

                    result.Add(item);
                    
                }
            }
            return result;
        }

        public Type TypeSupported { get { return typeof (string); }}
    }
}
