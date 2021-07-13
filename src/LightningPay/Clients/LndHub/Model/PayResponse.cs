namespace LightningPay.Clients.LndHub
{
    internal class PayResponse : ResponseBase
    {
        [Serializable("payment_error")]
        public string Error { get; set; }
    }
}
