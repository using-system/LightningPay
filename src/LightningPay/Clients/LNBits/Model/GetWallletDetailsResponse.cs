namespace LightningPay.Clients.LNBits
{
    internal class GetWallletDetailsResponse
    {
        [Serializable("id")]
        public string Id { get; set; }

        [Serializable("balance")]
        public long Balance { get; set; }
    }
}
