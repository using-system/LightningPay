namespace LightningPay.Clients.Lnd
{
    internal class GetBalanceResponse
    {
        [Json("total_balance")]
        public long TotalBalance { get; set; }
    }
}
