using System;

namespace LightningPay
{
    /// <summary>
    ///   Base class for client configuration options
    /// </summary>
    public abstract class OptionsBase
    {
        /// <summary>Gets or sets the address.</summary>
        /// <value>The address of the LND server.</value>
        public Uri Address { get; set; }
    }
}
