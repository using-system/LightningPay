using System;

namespace LightningPay
{
    /// <summary>
    ///   Invoice creation options
    /// </summary>
    public class CreateInvoiceOptions
    {
        /// <summary>Initializes a new instance of the <see cref="CreateInvoiceOptions" /> class.</summary>
        /// <param name="expiry">Invoice expiry.</param>
        public CreateInvoiceOptions(TimeSpan? expiry = null)
        {
            this.Expiry = expiry;
        }

        /// <summary>Gets the invoice expiry.</summary>
        /// <value>The invoice expiry.</value>
        public TimeSpan? Expiry { get; }
    }
}
