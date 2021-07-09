namespace LightningPay.Clients.LndHub
{
    internal class AddInvoiceRequest
    {
        [Json("amt")]
        public string Amount { get; set; }

        [Json("memo")]
        public string Memo { get; set; }

        [Json("expiry")]
        public string Expiry { get; set; } // not supported actually by the api. Actually hard coded with value : 3600 * 24 (1 day)
    }
}
