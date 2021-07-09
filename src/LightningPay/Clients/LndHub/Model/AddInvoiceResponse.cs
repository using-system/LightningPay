namespace LightningPay.Clients.LndHub
{
    internal class AddInvoiceResponse
    {
        [Json("r_hash")]
        public Hash R_hash { get; set; }

        [Json("payment_request")]
        public string PaymentRequest { get; set; }

        internal class Hash
        {
            [Json("type")]
            public string Type { get; set; }

            [Json("data")]
            public byte[] Data { get; set; }
        }
    }

}
