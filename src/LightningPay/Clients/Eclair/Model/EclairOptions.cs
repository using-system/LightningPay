using System;

namespace LightningPay.Clients.Eclair
{
    /// <summary>
    ///   Eclair options
    /// </summary>
    public class EclairOptions
    {
        /// <summary>Gets or sets the address.</summary>
        /// <value>The address of the Eclair server.</value>
        public Uri Address { get; set; }

        /// <summary>Gets or sets the password.</summary>
        /// <value>The password.</value>
        public string Password { get; set; }
    }
}
