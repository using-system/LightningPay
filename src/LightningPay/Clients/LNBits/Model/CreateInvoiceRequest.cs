namespace LightningPay.Clients.LNBits
{
    internal class CreateInvoiceRequest
    {
        [Serializable("out")]
        public bool Out { get; set; }

        [Serializable("amount")]
        public long Amount { get; set; }

        [Serializable("memo")]
        public string Memo { get; set; }
    }
}
