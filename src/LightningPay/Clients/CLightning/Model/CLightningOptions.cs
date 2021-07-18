using System;

namespace LightningPay.Clients.CLightning
{
    /// <summary>
    ///   C-Lightning Options
    /// </summary>
    public class CLightningOptions
    {
        /// <summary>Gets or sets the address.</summary>
        /// <value>The address of the LND server.</value>
        public Uri Address { get; set; }
    }
}
