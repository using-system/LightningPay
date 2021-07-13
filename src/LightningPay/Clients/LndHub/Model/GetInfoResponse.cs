namespace LightningPay.Clients.LndHub
{
    internal class GetInfoResponse
    {
        [Json("alias")]
        public string Alias { get; set; }
    }
}
