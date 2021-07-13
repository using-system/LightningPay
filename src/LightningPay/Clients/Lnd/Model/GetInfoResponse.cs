namespace LightningPay.Clients.Lnd
{
    internal class GetInfoResponse
    {
        [Json("alias")]
        public string Alias { get; set; }
    }
}
