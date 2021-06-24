using System;

namespace LightningPay.Clients.LndHub
{
    public class LndHubOptions
    {
        public Uri BaseUri { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }
    }
}
