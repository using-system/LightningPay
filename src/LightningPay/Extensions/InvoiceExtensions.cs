﻿using System;
using System.Globalization;

namespace LightningPay
{
    internal static class InvoiceExtensions
    {
        internal static string ToExpiryString(this CreateInvoiceOptions source)
        {
            TimeSpan expiry = Constants.INVOICE_DEFAULT_EXPIRY;

            if(source?.Expiry.HasValue == true)
            {
                expiry = source.Expiry.Value;
            }

            return Math.Round(expiry.TotalSeconds, 0).ToString(CultureInfo.InvariantCulture);
        }

        internal static DateTimeOffset ToExpiryDate(this CreateInvoiceOptions source)
        {
            if (source == null
                || !source.Expiry.HasValue)
            {
                return DateTimeOffset.UtcNow + Constants.INVOICE_DEFAULT_EXPIRY;
            }

            return DateTimeOffset.UtcNow + source.Expiry.Value;
        }
    }
}