namespace LightningPay
{
    /// <summary>
    ///   Payment Received Event
    /// </summary>
    public class InvoiceUpdatedEvent : LightningEvent
    {
        /// <summary>Gets or sets the invoice.</summary>
        /// <value>The invoice.</value>
        public LightningInvoice Invoice { get; set; }
    }
}
