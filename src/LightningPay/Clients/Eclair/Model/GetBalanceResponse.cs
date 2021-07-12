namespace LightningPay.Clients.Eclair
{
    internal class GetBalanceResponse
    {
        [Json("confirmed")]
        public long Confirmed { get; set; }
    }
}