namespace LightningPay.Clients.LNBits
{
    internal class PayResponse
    {
        [Serializable("payment_hash")]
        public string PaymentHash { get; set; }
    }
}
