namespace LightningPay.Clients.LndHub
{
    internal class GetTokenResponse : ResponseBase
    {
        [Serializable("access_token")]
        public string AccessToken { get; set; }

        [Serializable("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
