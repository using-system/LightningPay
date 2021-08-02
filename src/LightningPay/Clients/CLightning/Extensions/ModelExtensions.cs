using System;

namespace LightningPay.Clients.CLightning
{
    internal static class ModelExtensions
    {
        internal static LightningInvoice ToLightningInvoice(this CLightningInvoice source)
        {
            if (source == null)
            {
                return null;
            }

            return new LightningInvoice
            {
                Id = source.Label,
                Memo = source.Description,
                Amount = Money.FromMilliSatoshis(source.MilliSatoshi),
                BOLT11 = source.BOLT11,
                ExpiresAt = DateTimeOffset.FromUnixTimeSeconds(source.ExpiryAt),
                Status = source.ToStatus()            
            };
        }

        private static LightningInvoiceStatus ToStatus(this CLightningInvoice source)
        {
            switch (source.Status)
            {
                case "paid":
                    return LightningInvoiceStatus.Paid;
                case "unpaid":
                    return LightningInvoiceStatus.Unpaid;
                case "expired":
                    return LightningInvoiceStatus.Expired;
                default:
                    return LightningInvoiceStatus.Unpaid;
            }
        }

        public static PaymentSentEvent ToEvent(this CLightningInvoice source)
        {
            if (source == null)
            {
                return null;
            }

            return new PaymentSentEvent()
            {
                Invoice = source.ToLightningInvoice()
            };
        }
    }
}
