using LightningPay.Tools;

namespace LightningPay.Clients.LndHub
{
    internal class AddInvoiceResponse
    {
        [Json("r_hash")]
        public byte[] R_hash { get; set; }

        [Json("payment_request")]
        public string Payment_request { get; set; }
    }
}
