namespace LightningPay.Clients.LNBits
{
    internal class PayResponse
    {
        [Json("payment_hash")]
        public string PaymentHash { get; set; }
    }
}
