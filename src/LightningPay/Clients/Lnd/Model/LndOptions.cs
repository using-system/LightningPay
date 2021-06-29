using System;

namespace LightningPay.Clients.Lnd
{
    /// <summary>
    ///   LND options
    /// </summary>
    public class LndOptions
    {
        /// <summary>Gets or sets the address.</summary>
        /// <value>The address of the LND server.</value>
        public Uri Address { get; set; }

        /// <summary>Gets or sets the macaroon.</summary>
        /// <value>The authentication macaroon.</value>
        public byte[] Macaroon { get; set; }
    }
}
