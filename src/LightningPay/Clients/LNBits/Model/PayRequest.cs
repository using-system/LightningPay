namespace LightningPay.Clients.LNBits
{
    internal class PayRequest
    {
        [Json("out")]
        public bool Out { get; set; }

        [Json("bolt11")]
        public string PaymentRequest { get; set; }
    }
}
