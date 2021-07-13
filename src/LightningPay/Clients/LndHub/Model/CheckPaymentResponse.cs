namespace LightningPay.Clients.LndHub
{
    internal class CheckPaymentResponse : ResponseBase
    {
        [Serializable("paid")]
        public bool Paid { get; set; }
    }
}
