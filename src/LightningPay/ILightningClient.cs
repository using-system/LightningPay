using System;
using System.Threading.Tasks;

namespace LightningPay
{
    public interface ILightningClient
    {
        Task<LightningInvoice> CreateInvoice(long satoshis, string description, TimeSpan expiry);

        Task<bool> CheckPayment(string invoiceId);
    }
}
