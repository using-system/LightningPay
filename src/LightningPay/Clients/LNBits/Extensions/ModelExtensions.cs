namespace LightningPay.Clients.LNBits
{
    internal static class ModelExtensions
    {
        internal static LightningInvoice ToLightningInvoice(this CreateInvoiceResponse source,
          Money amount,
          string memo,
          CreateInvoiceOptions options)
        {
            if (source == null)
            {
                return null;
            }

            return new LightningInvoice
            {
                Id = source.PaymentHash,
                Memo = memo,
                Amount = amount,
                BOLT11 = source.PaymentRequest,
                Status = LightningInvoiceStatus.Unpaid,
                ExpiresAt = options.ToExpiryDate()
            };
        }
    }
}
