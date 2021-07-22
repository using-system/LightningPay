namespace LightningPay
{
    /// <summary>
    ///   Payment Received Event
    /// </summary>
    public class PaymentReceivedEvent
    {
        /// <summary>Gets or sets the amount.</summary>
        /// <value>The amount.</value>
        public Money Amount { get; set; }

        /// <summary>Gets or sets the payment hash.</summary>
        /// <value>The payment hash.</value>
        public string PaymentHash { get; set; }

        /// <summary>Gets or sets the timestamp.</summary>
        /// <value>The timestamp.</value>
        public long Timestamp { get; set; }
    }
}
