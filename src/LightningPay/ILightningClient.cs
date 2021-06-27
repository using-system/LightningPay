using System;
using System.Threading.Tasks;

namespace LightningPay
{
    public interface ILightningClient : IDisposable
    {
        Task<LightningInvoice> CreateInvoice(long satoshis, string description, CreateInvoiceOptions options = null);

        Task<bool> CheckPayment(string invoiceId);
    }
}
