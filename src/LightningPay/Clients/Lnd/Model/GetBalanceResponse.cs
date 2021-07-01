using LightningPay.Tools;

namespace LightningPay.Clients.Lnd
{
    internal class GetBalanceResponse
    {
        [Json("total_balance")]
        internal long TotalBalance { get; set; }
    }
}
