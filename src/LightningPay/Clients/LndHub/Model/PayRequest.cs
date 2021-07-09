namespace LightningPay.Clients.LndHub{
    internal class PayRequest
    {
        [Json("invoice")]
        public string PaymentRequest { get; set; }
    }
}
