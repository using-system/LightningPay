namespace LightningPay.Clients.Eclair
{
    internal class CreateInvoiceRequest
    {
        [Serializable("amountMsat")]
        public long AmountMsat { get; set; }

        [Serializable("description")]
        public string Description { get; set; }

        [Serializable("expireIn")]
        public int ExpireIn { get; set; }
    }
}
