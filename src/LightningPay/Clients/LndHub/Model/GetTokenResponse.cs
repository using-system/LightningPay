namespace LightningPay.Clients.LndHub
{
    internal class GetTokenResponse : ResponseBase
    {
        [Json("access_token")]
        public string AccessToken { get; set; }

        [Json("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
