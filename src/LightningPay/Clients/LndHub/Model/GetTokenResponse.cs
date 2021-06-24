using LightningPay.Tools;

namespace LightningPay.Clients.LndHub
{
    internal class GetTokenResponse
    {
        [Json("access_token")]
        public string AccessToken { get; set; }
    }
}
