using Newtonsoft.Json;

namespace LightningPay.Clients.Lnd
{
    internal class AddInvoiceResponse
    {
        [JsonProperty("r_hash",
            Required = Required.Default, 
            NullValueHandling = NullValueHandling.Ignore)]
        public byte[] R_hash { get; set; }

        [JsonProperty("payment_request", 
            Required = Required.Default, 
            NullValueHandling = NullValueHandling.Ignore)]
        public string Payment_request { get; set; }
    }
}
