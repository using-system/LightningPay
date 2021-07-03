using System;

namespace LightningPay.Clients.LNBits
{
    /// <summary>
    ///   LNBits Options
    /// </summary>
    public class LNBitsOptions
    {
        /// <summary>Gets or sets the address.</summary>
        /// <value>The address of the LNBits server.</value>
        public Uri Address { get; set; }

        /// <summary>Gets or sets the API key.</summary>
        /// <value>The API key.</value>
        public string ApiKey { get; set; }
    }
}
