using System;
using System.Threading.Tasks;

namespace LightningPay
{
    public interface ILightningClient : IDisposable
    {
        Task<LightningInvoice> CreateInvoice(long satoshis, string description, TimeSpan expiry);

        Task<bool> CheckPayment(string invoiceId);
    }
}
