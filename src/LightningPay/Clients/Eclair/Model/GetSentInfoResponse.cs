using System.Collections.Generic;

namespace LightningPay.Clients.Eclair
{
    internal class GetSentInfoResponse : List<SentInfoResponse>
    {

    }

    internal class SentInfoResponse
    {
        [Serializable("amount")]
        public long Amount { get; set; }

        [Serializable("status")]
        public GetReceivedInfoStatusResponse Status { get; set; }

        internal class GetReceivedInfoStatusResponse
        {
            [Serializable("type")]
            public string Type { get; set; }
        }
    }
}
