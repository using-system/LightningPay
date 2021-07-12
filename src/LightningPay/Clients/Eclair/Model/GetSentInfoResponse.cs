using System.Collections.Generic;

namespace LightningPay.Clients.Eclair
{
    internal class GetSentInfoResponse : List<SentInfoResponse>
    {

    }

    internal class SentInfoResponse
    {
        [Json("amount")]
        public long Amount { get; set; }

        [Json("status")]
        public GetReceivedInfoStatusResponse Status { get; set; }

        internal class GetReceivedInfoStatusResponse
        {
            [Json("type")]
            public string Type { get; set; }
        }
    }
}
