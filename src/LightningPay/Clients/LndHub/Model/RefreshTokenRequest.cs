namespace LightningPay.Clients.LndHub
{
    internal class RefreshTokenRequest
    {
        [Serializable("refresh_token")]
        public string ResfreshToken { get; set; }
    }
}
