using LightningPay.Tools;

namespace LightningPay.Clients.LndHub
{
    internal class GetTokenRequest
    {
        [Json("login")]
        public string Login { get; set; }

        [Json("password")]
        public string Password { get; set; }
    }
}
