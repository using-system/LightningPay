using LightningPay.Tools;

namespace LightningPay.Clients.LNBits
{
    internal class CheckPaymentResponse
    {
        [Json("paid")]
        public bool Paid { get; set; }
    }
}
