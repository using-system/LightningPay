namespace LightningPay.Clients.Eclair
{
    internal class GetSentInfoRequest
    {
        [Json("id")]
        public string Id { get; set; }
    }
}
