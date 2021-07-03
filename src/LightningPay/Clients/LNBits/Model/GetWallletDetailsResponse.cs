using LightningPay.Tools;

namespace LightningPay.Clients.LNBits
{
    internal class GetWallletDetailsResponse
    {
        [Json("id")]
        public string Id { get; set; }

        [Json("balance")]
        public long Balance { get; set; }
    }
}
