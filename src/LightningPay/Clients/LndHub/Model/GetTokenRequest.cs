namespace LightningPay.Clients.LndHub
{
    internal class GetTokenRequest
    {
        [Serializable("login")]
        public string Login { get; set; }

        [Serializable("password")]
        public string Password { get; set; }
    }
}
