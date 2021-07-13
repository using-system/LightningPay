using System;

namespace LightningPay
{
    /// <summary>
    ///   Lightning Invoice
    /// </summary>
    public class LightningInvoice
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>Gets or sets the memo.</summary>
        /// <value>The memo.</value>
        public string Memo { get; set; }

        /// <summary>Gets or sets the status.</summary>
        /// <value>The status.</value>
        public LightningInvoiceStatus Status { get; set; }

        /// <summary>Gets or sets the payment request.</summary>
        /// <value>The payment request.</value>
        public string BOLT11 { get; set; }

        /// <summary>Gets or sets the expires at.</summary>
        /// <value>The expires at.</value>
        public DateTimeOffset ExpiresAt { get; set; }

        /// <summary>Gets or sets the amount.</summary>
        /// <value>The amount.</value>
        public Money Amount { get; set; }

        /// <summary>Gets the URI.</summary>
        /// <value>The URI.</value>
        public string Uri
        {
            get
            {
                return $"lightning:{BOLT11}";
            }
        }
    }
}
