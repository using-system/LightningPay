namespace LightningPay.Clients.LNBits
{
    internal class CreateInvoiceResponse
    {
        [Json("payment_hash")]
        public string PaymentHash { get; set; }

        [Json("payment_request")]
        public string PaymentRequest { get; set; }
    }
}
