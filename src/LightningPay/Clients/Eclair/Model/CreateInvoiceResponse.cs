namespace LightningPay.Clients.Eclair
{
    internal class CreateInvoiceResponse
    {
        [Serializable("description")]
        public string Description { get; set; }

        [Serializable("paymentHash")]
        public string PaymentHash { get; set; }

        [Serializable("amount")]
        public long Amount { get; set; }

        [Serializable("expiry")]
        public int Expiry { get; set; }

        [Serializable("serialized")]
        public string Serialized { get; set; }

        [Serializable("createdAt")]
        public long CreatedAt { get; set; }
    }
}
