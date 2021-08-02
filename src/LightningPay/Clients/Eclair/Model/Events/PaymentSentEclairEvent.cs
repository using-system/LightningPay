namespace LightningPay.Clients.Eclair
{
    internal class PaymentSentEclairEvent : EclairEvent
    {
        [Serializable("paymentHash")]
        public string PaymentHash { get; set; }
    }
}
