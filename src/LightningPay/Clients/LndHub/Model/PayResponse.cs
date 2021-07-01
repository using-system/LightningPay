using LightningPay.Tools;

namespace LightningPay.Clients.LndHub
{
    internal class PayResponse
    {
        [Json("payment_error")]
        public string Error { get; set; }
    }
}
