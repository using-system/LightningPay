namespace LightningPay.Clients.Lnd
{
    internal class PayRequest
    {
        [Serializable("payment_request")]
        public string PaymentRequest { get; set; }
    }
}
