using LightningPay.Tools;

namespace LightningPay.Clients.LndHub
{
    public class GetTokenResponse
    {
        [Json("access_token")]
        public string AccessToken { get; set; }
    }
}
