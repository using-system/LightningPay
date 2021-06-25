using System;
using System.Threading.Tasks;

namespace LightningPay
{
    public interface ILightningClient
    {
        Task<LightningInvoice> CreateInvoice(LightMoney money, string description, TimeSpan expiry);

        Task<bool> CheckPayment(string invoiceId);

        Task<LightningInvoice> GetInvoice(string invoiceId);
    }
}
