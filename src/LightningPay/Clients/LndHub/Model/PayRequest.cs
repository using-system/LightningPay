namespace LightningPay.Clients.LndHub{
    internal class PayRequest
    {
        [Serializable("invoice")]
        public string PaymentRequest { get; set; }
    }
}
