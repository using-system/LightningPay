namespace LightningPay.Clients.Lnd
{
    internal class PayRequest
    {
        [Json("payment_request")]
        public string PaymentRequest { get; set; }
    }
}
