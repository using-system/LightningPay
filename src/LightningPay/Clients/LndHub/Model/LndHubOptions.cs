using System;

namespace LightningPay.Clients.LndHub
{
    /// <summary>
    ///   LNDHub options
    /// </summary>
    public class LndHubOptions
    {
        /// <summary>Gets or sets the address.</summary>
        /// <value>The address of the LNDHub.</value>
        public Uri Address { get; set; }

        /// <summary>Gets or sets the login.</summary>
        /// <value>The login.</value>
        public string Login { get; set; }

        /// <summary>Gets or sets the password.</summary>
        /// <value>The password.</value>
        public string Password { get; set; }
    }
}
