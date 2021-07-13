namespace LightningPay.Clients.LndHub
{
    internal class AddInvoiceResponse : ResponseBase
    {
        [Serializable("r_hash")]
        public Hash R_hash { get; set; }

        [Serializable("payment_request")]
        public string PaymentRequest { get; set; }

        internal class Hash
        {
            [Serializable("type")]
            public string Type { get; set; }

            [Serializable("data")]
            public byte[] Data { get; set; }
        }
    }

}
