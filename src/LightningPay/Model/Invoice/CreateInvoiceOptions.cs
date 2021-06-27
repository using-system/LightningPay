using System;

namespace LightningPay
{
    public class CreateInvoiceOptions
    {
        public CreateInvoiceOptions(TimeSpan? expiry = null)
        {
            this.Expiry = expiry;
        }

        public TimeSpan? Expiry { get; }
    }
}
