using System;

namespace LightningPay.Clients.Eclair
{
    internal static class ModelExtensions
    {
        public static LightningInvoice ToLightningInvoice(this CreateInvoiceResponse source)
        {
            if (source == null)
            {
                return null;
            }

            return new LightningInvoice()
            {
                Id = source.PaymentHash,
                Memo = source.Description,
                Amount = source.Amount / 1000,
                Status = LightningInvoiceStatus.Unpaid,
                BOLT11 = source.Serialized,
                ExpiresAt = DateTimeOffset.FromUnixTimeMilliseconds(source.CreatedAt)
                    .AddSeconds(source.Expiry)
            };
        }
    }
}
