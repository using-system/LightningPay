using System;

namespace LightningPay.Clients.Lnd
{
    public class LndOptions
    {
        public Uri BaseUri { get; set; }

        public byte[] Macaroon { get; set; }
    }
}
