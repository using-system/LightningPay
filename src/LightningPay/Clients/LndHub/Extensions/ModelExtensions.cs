using System;

namespace LightningPay.Clients.LndHub
{
    internal static class ModelExtensions
    {
        public static LightningInvoice ToLightningInvoice(this AddInvoiceResponse source,
          LightMoney amount,
          string memo,
          TimeSpan expiry)
        {
            if (source == null)
            {
                return null;
            }

            return new LightningInvoice
            {
                Id = source.R_hash.Data.ToBitString(),
                Memo = memo,
                Amount = amount,
                BOLT11 = source.PaymentRequest,
                Status = LightningInvoiceStatus.Unpaid,
                ExpiresAt = DateTimeOffset.UtcNow + expiry
            };
        }
    }
}
