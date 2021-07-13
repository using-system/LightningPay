namespace LightningPay.Clients.Lnd
{
    internal class PayResponse
    {
        [Serializable("payment_error")]
        public string Error { get; set; }
    }
}
