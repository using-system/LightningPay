namespace LightningPay.Clients.LNBits
{
    internal class PayRequest
    {
        [Serializable("out")]
        public bool Out { get; set; }

        [Serializable("bolt11")]
        public string PaymentRequest { get; set; }
    }
}
