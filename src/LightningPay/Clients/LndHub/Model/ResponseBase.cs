namespace LightningPay.Clients.LndHub
{
    internal abstract class ResponseBase
    {
        [Json("error")]
        public bool Failed { get; set; }

        [Json("message")]
        public string Message { get; set; }
    }
}
