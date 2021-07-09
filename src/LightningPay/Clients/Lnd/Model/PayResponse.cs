namespace LightningPay.Clients.Lnd
{
    internal class PayResponse
    {
        [Json("payment_error")]
        public string Error { get; set; }
    }
}
