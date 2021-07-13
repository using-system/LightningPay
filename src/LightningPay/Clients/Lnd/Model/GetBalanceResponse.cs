namespace LightningPay.Clients.Lnd
{
    internal class GetBalanceResponse
    {
        [Serializable("total_balance")]
        public long TotalBalance { get; set; }
    }
}
