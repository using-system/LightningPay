namespace LightningPay.Clients.LNBits
{
    internal class CreateInvoiceRequest
    {
        [Json("out")]
        public bool Out { get; set; }

        [Json("amount")]
        public long Amount { get; set; }

        [Json("memo")]
        public string Memo { get; set; }
    }
}
