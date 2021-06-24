using LightningPay.Tools;

namespace LightningPay.Clients.LndHub
{
    internal class AddInvoiceRequest
    {
        [Json("amt")]
        public string Amount { get; set; }

        [Json("memo")]
        public string Memo { get; set; }

        [Json("expiry")]
        public string Expiry { get; set; }
    }
}
