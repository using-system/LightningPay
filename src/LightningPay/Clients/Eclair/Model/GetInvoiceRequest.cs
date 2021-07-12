namespace LightningPay.Clients.Eclair
{
    internal class GetInvoiceRequest
    {
        [Json("paymentHash")]
        public string PaymentHash { get; set; }
    }
}
