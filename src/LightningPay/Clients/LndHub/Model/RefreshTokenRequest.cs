namespace LightningPay.Clients.LndHub
{
    internal class RefreshTokenRequest
    {
        [Json("refresh_token")]
        public string ResfreshToken { get; set; }
    }
}
