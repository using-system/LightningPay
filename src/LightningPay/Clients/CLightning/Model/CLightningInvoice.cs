using System;

namespace LightningPay.Clients.CLightning
{
    internal class CLightningInvoice
    {
        [Serializable("payment_hash")]
        public string PaymentHash { get; set; }

        [Serializable("msatoshi")]
        public long MilliSatoshi { get; set; }

        [Serializable("msatoshi_received")]
        public long MilliSatoshiReceived { get; set; }

        [Serializable("expiry_time")]
        public DateTimeOffset ExpiryTime { get; set; }

        [Serializable("expires_at")]
        public DateTimeOffset ExpiryAt { get; set; }

        [Serializable("bolt11")]
        public string BOLT11 { get; set; }

        [Serializable("pay_index")]
        public int? PayIndex { get; set; }

        public string Label { get; set; }

        public string Status { get; set; }

        [Serializable("paid_at")]
        public DateTimeOffset? PaidAt { get; set; }
    }
}
