namespace LightningPay.Clients.LndHub
{
    internal class GetBalanceResponse
    {
        [Json("BTC")]
        public BTCBalance BTC { get; set; }

        internal class BTCBalance
        {
            [Json("AvailableBalance")]
            public long AvailableBalance { get; set; }
        }
    }
}
