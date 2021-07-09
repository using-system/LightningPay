namespace LightningPay.Clients.Lnd
{
    internal class LnrpcInvoice
    {
        [Json("memo")]
        public string Memo { get; set; }

        [Json("receipt")]
        public byte[] Receipt { get; set; }

        [Json("r_preimage")]
        public byte[] R_preimage { get; set; }

        [Json("r_hash")]
        public byte[] R_hash { get; set; }

        [Json("value")]
        public string Value { get; set; }

        [Json("amt_paid_msat")]
        public string AmountPaid { get; set; }

        [Json("settled")]
        public bool? Settled { get; set; }

        [Json("creation_date")]
        public string Creation_date { get; set; }

        [Json("settle_date")]
        public string Settle_date { get; set; }

        [Json("payment_request")]
        public string Payment_request { get; set; }

        [Json("description_hash")]
        public byte[] Description_hash { get; set; }

        [Json("expiry")]
        public string Expiry { get; set; }


        [Json("fallback_addr")]
        public string Fallback_addr { get; set; }


        [Json("cltv_expiry")]
        public string Cltv_expiry { get; set; }

        [Json("private")]
        public bool? Private { get; set; }

    }
}
