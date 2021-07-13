namespace LightningPay.Clients.Eclair
{
    internal class GetReceivedInfoRequest
    {
        [Serializable("paymentHash")]
        public string PaymentHash { get; set; }
    }
}
