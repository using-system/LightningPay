namespace LightningPay
{
    /// <summary>
    ///   Payment Received Event
    /// </summary>
    public class PaymentReceivedEvent : LightningEvent
    {
        /// <summary>Gets or sets the invoice.</summary>
        /// <value>The invoice.</value>
        public LightningInvoice Invoice { get; set; }
    }
}
