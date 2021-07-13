namespace LightningPay.Clients.Lnd
{
    internal class AddInvoiceResponse
    {
        [Serializable("r_hash")]
        public byte[] R_hash { get; set; }

        [Serializable("payment_request")]
        public string Payment_request { get; set; }
    }
}
