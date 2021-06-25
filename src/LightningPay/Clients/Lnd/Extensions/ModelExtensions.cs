using System;
using System.Globalization;

namespace LightningPay.Clients.Lnd
{
    internal static class ModelExtensions
    {
        public static LightningInvoice ToLightningInvoice(this AddInvoiceResponse source,
            long amount,
            string memo,
            TimeSpan expiry)
        {
            if(source == null)
            {
                return null;
            }

            return new LightningInvoice
            {
                Id = source.R_hash.ToBitString(),
                Memo = memo,
                Amount = amount,
                BOLT11 = source.Payment_request,
                Status = LightningInvoiceStatus.Unpaid,
                ExpiresAt = DateTimeOffset.UtcNow + expiry
            };
        }

        public static LightningInvoice ToLightningInvoice(this LnrpcInvoice source)
        {
            if(source == null)
            {
                return null;
            }

            var invoice = new LightningInvoice
            {
                Id =  source.R_hash.ToBitString(),
                Memo = source.Memo,
                Amount = Convert.ToInt64(source.Value, CultureInfo.InvariantCulture.NumberFormat),
                BOLT11 = source.Payment_request,
                Status = LightningInvoiceStatus.Unpaid
            };

            invoice.ExpiresAt = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(source.Creation_date, CultureInfo.InvariantCulture.NumberFormat) 
                + Convert.ToInt64(source.Expiry, CultureInfo.InvariantCulture.NumberFormat));;
            
            if (source.Settled == true)
            {
                invoice.Status = LightningInvoiceStatus.Paid;
            }
            else
            {
                if (invoice.ExpiresAt < DateTimeOffset.UtcNow)
                {
                    invoice.Status = LightningInvoiceStatus.Expired;
                }
            }
            return invoice;
        }
    }
}
