namespace LightningPay.Clients.Eclair
{
    internal class PayRequest
    {
        [Json("invoice")]
        public string PaymentRequest { get; set; }
    }
}
