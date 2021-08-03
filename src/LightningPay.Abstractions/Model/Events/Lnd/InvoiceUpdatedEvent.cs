namespace LightningPay.Events.Lnd
{
    /// <summary>
    ///   Invoice updated event on LND node
    /// </summary>
    public class InvoiceUpdatedEvent : LightningEvent
    {
        /// <summary>Gets or sets the invoice.</summary>
        /// <value>The invoice.</value>
        public LightningInvoice Invoice { get; set; }
    }
   
}
