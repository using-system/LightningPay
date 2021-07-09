namespace LightningPay.Clients.LndHub
{
    internal class GetTokenResponse
    {
        [Json("access_token")]
        public string AccessToken { get; set; }

        [Json("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
