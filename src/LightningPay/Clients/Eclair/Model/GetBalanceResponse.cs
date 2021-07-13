namespace LightningPay.Clients.Eclair
{
    internal class GetBalanceResponse
    {
        [Serializable("confirmed")]
        public long Confirmed { get; set; }
    }
}