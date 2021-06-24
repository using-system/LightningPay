using System;

namespace LightningPay
{
    public class LightningInvoice
    {
        public string Id { get; set; }

        public string Memo { get; set; }

        public LightningInvoiceStatus Status { get; set; }

        public string BOLT11 { get; set; }

        public DateTimeOffset? PaidAt { get; set; }

        public DateTimeOffset ExpiresAt { get; set; }
        
        public LightMoney Amount { get; set; }
        
        public LightMoney AmountReceived { get; set; }

        public string Uri
        {
            get
            {
                return $"lightning:{BOLT11}";
            }
        }
    }
}
