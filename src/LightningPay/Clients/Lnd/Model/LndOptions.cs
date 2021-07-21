﻿namespace LightningPay.Clients.Lnd
{
    /// <summary>
    ///   LND options
    /// </summary>
    public class LndOptions : OptionsBase
    {
        /// <summary>Gets or sets the macaroon.</summary>
        /// <value>The authentication macaroon.</value>
        public byte[] Macaroon { get; set; }
    }
}
