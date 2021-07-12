namespace LightningPay.Clients.Eclair
{
    internal static class ModelExtensions
    {
        public static LightningInvoice ToLightningInvoice(this GetInvoiceResponse source)
        {
            if (source == null)
            {
                return null;
            }

            return new LightningInvoice();
        }
    }
}
