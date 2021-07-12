namespace LightningPay.Clients.Eclair
{
    internal class CreateInvoiceResponse
    {
        [Json("description")]
        public string Description { get; set; }

        [Json("paymentHash")]
        public string PaymentHash { get; set; }

        [Json("amount")]
        public long Amount { get; set; }

        [Json("expiry")]
        public int Expiry { get; set; }

        [Json("serialized")]
        public string Serialized { get; set; }

        [Json("createdAt")]
        public long CreatedAt { get; set; }
    }
}
