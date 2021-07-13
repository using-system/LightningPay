namespace LightningPay.Clients.Eclair
{
    internal class GetReceivedInfoResponse
    {
        [Serializable("amount")]
        public long Amount { get; set; }

        [Serializable("status")]
        public GetReceivedInfoStatusResponse Status { get; set; }

        internal class GetReceivedInfoStatusResponse
        {
            [Serializable("type")]
            public string Type { get; set; }

            [Serializable("amount")]
            public long Amount { get; set; }
        }
    }
}
