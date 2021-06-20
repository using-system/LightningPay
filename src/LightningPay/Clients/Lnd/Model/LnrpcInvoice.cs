using Newtonsoft.Json;

namespace LightningPay.Clients.Lnd
{
    public class LnrpcInvoice
    {
        [JsonProperty("memo", 
            Required = Required.Default, 
            NullValueHandling = NullValueHandling.Ignore)]
        public string Memo { get; set; }

        [JsonProperty("receipt", 
            Required = Required.Default, 
            NullValueHandling = NullValueHandling.Ignore)]
        public byte[] Receipt { get; set; }

        [JsonProperty("r_preimage", 
            Required = Required.Default, 
            NullValueHandling = NullValueHandling.Ignore)]
        public byte[] R_preimage { get; set; }

        [JsonProperty("r_hash", 
            Required = Required.Default, 
            NullValueHandling = NullValueHandling.Ignore)]
        public byte[] R_hash { get; set; }

        [JsonProperty("value", 
            Required = Required.Default, 
            NullValueHandling =NullValueHandling.Ignore)]
        public string Value { get; set; }

        [JsonProperty("amt_paid_msat", 
            Required = Required.Default, 
            NullValueHandling = NullValueHandling.Ignore)]
        public string AmountPaid { get; set; }

        [JsonProperty("settled", 
            Required = Required.Default, 
            NullValueHandling = NullValueHandling.Ignore)]
        public bool? Settled { get; set; }

        [JsonProperty("creation_date", 
            Required = Required.Default, 
            NullValueHandling = NullValueHandling.Ignore)]
        public string Creation_date { get; set; }

        [JsonProperty("settle_date", 
            Required =Required.Default, 
            NullValueHandling = NullValueHandling.Ignore)]
        public string Settle_date { get; set; }

        [JsonProperty("payment_request", 
            Required = Required.Default, 
            NullValueHandling = NullValueHandling.Ignore)]
        public string Payment_request { get; set; }

        [JsonProperty("description_hash", 
            Required = Required.Default, 
            NullValueHandling = NullValueHandling.Ignore)]
        public byte[] Description_hash { get; set; }

        [JsonProperty("expiry", 
            Required = Required.Default, 
            NullValueHandling = NullValueHandling.Ignore)]
        public string Expiry { get; set; }


        [JsonProperty("fallback_addr", 
            Required = Required.Default, 
            NullValueHandling = NullValueHandling.Ignore)]
        public string Fallback_addr { get; set; }


        [JsonProperty("cltv_expiry", 
            Required = Required.Default, 
            NullValueHandling = NullValueHandling.Ignore)]
        public string Cltv_expiry { get; set; }

        [JsonProperty("private", 
            Required = Required.Default, 
            NullValueHandling = NullValueHandling.Ignore)]
        public bool? Private { get; set; }

    }
}
