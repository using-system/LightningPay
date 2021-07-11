namespace LightningPay.Clients.LndHub
{
    internal class CheckPaymentResponse : ResponseBase
    {
        [Json("paid")]
        public bool Paid { get; set; }
    }
}
