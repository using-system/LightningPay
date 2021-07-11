namespace LightningPay.Clients.LndHub
{
    internal class PayResponse : ResponseBase
    {
        [Json("payment_error")]
        public string Error { get; set; }
    }
}
