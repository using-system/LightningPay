namespace LightningPay.Clients.LndHub
{
    internal class GetBalanceResponse : ResponseBase
    {
        [Serializable("BTC")]
        public BTCBalance BTC { get; set; }

        internal class BTCBalance
        {
            [Serializable("AvailableBalance")]
            public long AvailableBalance { get; set; }
        }
    }
}
