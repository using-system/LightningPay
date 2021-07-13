namespace LightningPay.Clients.Lnd
{
    internal class LnrpcInvoice
    {
        [Serializable("memo")]
        public string Memo { get; set; }

        [Serializable("receipt")]
        public byte[] Receipt { get; set; }

        [Serializable("r_preimage")]
        public byte[] R_preimage { get; set; }

        [Serializable("r_hash")]
        public byte[] R_hash { get; set; }

        [Serializable("value")]
        public string Value { get; set; }

        [Serializable("amt_paid_msat")]
        public string AmountPaid { get; set; }

        [Serializable("settled")]
        public bool? Settled { get; set; }

        [Serializable("creation_date")]
        public string Creation_date { get; set; }

        [Serializable("settle_date")]
        public string Settle_date { get; set; }

        [Serializable("payment_request")]
        public string Payment_request { get; set; }

        [Serializable("description_hash")]
        public byte[] Description_hash { get; set; }

        [Serializable("expiry")]
        public string Expiry { get; set; }


        [Serializable("fallback_addr")]
        public string Fallback_addr { get; set; }


        [Serializable("cltv_expiry")]
        public string Cltv_expiry { get; set; }

        [Serializable("private")]
        public bool? Private { get; set; }

    }
}
