namespace LightningPay.Clients.Eclair
{
    internal class GetReceivedInfoRequest
    {
        [Json("paymentHash")]
        public string PaymentHash { get; set; }
    }
}
