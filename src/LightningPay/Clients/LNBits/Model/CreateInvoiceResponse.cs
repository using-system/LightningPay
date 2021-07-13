namespace LightningPay.Clients.LNBits
{
    internal class CreateInvoiceResponse
    {
        [Serializable("payment_hash")]
        public string PaymentHash { get; set; }

        [Serializable("payment_request")]
        public string PaymentRequest { get; set; }
    }
}
