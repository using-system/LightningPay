using System;
using System.Globalization;

namespace LightningPay
{
    /// <summary>
    ///   Invoice exntension methods
    /// </summary>
    public static class InvoiceExtensions
    {
        /// <summary>Converts to expirystring.</summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   Expiry string
        /// </returns>
        public static string ToExpiryString(this CreateInvoiceOptions source)
        {
            TimeSpan expiry = Constants.INVOICE_DEFAULT_EXPIRY;

            if(source?.Expiry.HasValue == true)
            {
                expiry = source.Expiry.Value;
            }

            return Math.Round(expiry.TotalSeconds, 0).ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>Converts to expirydate.</summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   Expiry date
        /// </returns>
        public static DateTimeOffset ToExpiryDate(this CreateInvoiceOptions source)
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
