using System;

namespace UmbracoVault.Exceptions
{
    /// <summary>
    /// Custom exception to indicate that Vault specifically and intentionally does not implement a piece of funcationality.
    /// </summary>
    public class VaultNotImplementedException : NotImplementedException
    {
        public VaultNotImplementedException()
        {
        }

        public VaultNotImplementedException(string message)
            :base(message)
        {
        }
    }
}
