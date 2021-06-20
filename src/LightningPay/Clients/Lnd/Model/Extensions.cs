using System;
using System.Globalization;

namespace LightningPay.Clients.Lnd
{
    public static class Extensions
    {
        public static LightningInvoice ToLightningInvoice(this AddInvoiceResponse source,
            LightMoney amount,
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
                Amount = new LightMoney(Convert.ToInt64(source.Value, CultureInfo.InvariantCulture.NumberFormat), LightMoneyUnit.Satoshi), //new LightMoney(ConvertInv.ToInt64(resp.Value), LightMoneyUnit.Satoshi),
                AmountReceived = string.IsNullOrWhiteSpace(source.AmountPaid) ? null : 
                    new LightMoney(Convert.ToInt64(source.AmountPaid, CultureInfo.InvariantCulture.NumberFormat), LightMoneyUnit.MilliSatoshi),
                BOLT11 = source.Payment_request,
                Status = LightningInvoiceStatus.Unpaid
            };

            invoice.ExpiresAt = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(source.Creation_date, CultureInfo.InvariantCulture.NumberFormat) 
                + Convert.ToInt64(source.Expiry, CultureInfo.InvariantCulture.NumberFormat));;
            
            if (source.Settled == true)
            {
                invoice.PaidAt = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(source.Settle_date, CultureInfo.InvariantCulture.NumberFormat));

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

        public  static string ToBitString(this byte[] source)
        {
            return BitConverter.ToString(source)
                .Replace("-", "")
                .ToLower(CultureInfo.InvariantCulture);
        }
    }
}
