namespace LightningPay.Clients.Eclair
{
    internal class GetReceivedInfoResponse
    {
        [Json("amount")]
        public long Amount { get; set; }

        [Json("status")]
        public GetReceivedInfoStatusResponse Status { get; set; }

        internal class GetReceivedInfoStatusResponse
        {
            [Json("type")]
            public string Type { get; set; }

            [Json("amount")]
            public long Amount { get; set; }
        }
    }
}
