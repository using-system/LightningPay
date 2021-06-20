using System;
using System.Threading.Tasks;

namespace LightningPay
{
    public interface ILightningClient
    {
        Task<LightningInvoice> GetInvoice(string invoiceId);

        Task<LightningInvoice> CreateInvoice(LightMoney money, string description, TimeSpan expiry);
    }
}
