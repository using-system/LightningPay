using Newtonsoft.Json;

namespace LightningPay.Core.Clients.Lnd
{
    public class AddInvoiceResponse
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
