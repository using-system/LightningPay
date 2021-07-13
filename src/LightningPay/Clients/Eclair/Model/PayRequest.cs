namespace LightningPay.Clients.Eclair
{
    internal class PayRequest
    {
        [Serializable("invoice")]
        public string PaymentRequest { get; set; }
    }
}
