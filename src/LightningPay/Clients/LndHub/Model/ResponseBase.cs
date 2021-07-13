namespace LightningPay.Clients.LndHub
{
    internal abstract class ResponseBase
    {
        [Serializable("error")]
        public bool Failed { get; set; }

        [Serializable("message")]
        public string Message { get; set; }
    }
}
