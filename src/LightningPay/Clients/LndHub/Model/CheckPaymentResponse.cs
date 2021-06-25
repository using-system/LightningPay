using LightningPay.Tools;

namespace LightningPay.Clients.LndHub
{
    internal class CheckPaymentResponse
    {
        [Json("paid")]
        public bool Paid { get; set; }
    }
}
