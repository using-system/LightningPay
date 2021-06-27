using System;

namespace LightningPay.Clients.Lnd
{
    public class LndOptions
    {
        public Uri Address { get; set; }

        public byte[] Macaroon { get; set; }
    }
}
