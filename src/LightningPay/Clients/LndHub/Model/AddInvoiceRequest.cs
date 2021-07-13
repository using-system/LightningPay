namespace LightningPay.Clients.LndHub
{
    internal class AddInvoiceRequest
    {
        [Serializable("amt")]
        public string Amount { get; set; }

        [Serializable("memo")]
        public string Memo { get; set; }

        [Serializable("expiry")]
        public string Expiry { get; set; } // not supported actually by the api. Actually hard coded with value : 3600 * 24 (1 day)
    }
}
