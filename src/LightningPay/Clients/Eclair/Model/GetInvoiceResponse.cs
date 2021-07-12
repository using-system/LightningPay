namespace LightningPay.Clients.Eclair
{
    internal class GetInvoiceResponse
    {
        [Json("prefix")]
        public string Prefix { get; set; }

        [Json("serialized")]
        public string Serialized { get; set; }
    }
}
