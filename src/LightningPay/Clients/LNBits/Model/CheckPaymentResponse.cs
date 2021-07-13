namespace LightningPay.Clients.LNBits
{
    internal class CheckPaymentResponse
    {
        [Serializable("paid")]
        public bool Paid { get; set; }
    }
}
