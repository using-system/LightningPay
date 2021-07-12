namespace LightningPay.Clients.Eclair
{
    internal class CreateInvoiceRequest
    {
        [Json("amountMsat")]
        public long AmountMsat { get; set; }

        [Json("description")]
        public string Description { get; set; }

        [Json("expireIn")]
        public int ExpireIn { get; set; }
    }
}
