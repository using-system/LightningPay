using System;
using System.Threading.Tasks;

namespace LightningPay
{
    public interface ILightningClient
    {
        Task<LightningInvoice> GetInvoice(string invoiceId);

        Task<string> CreateInvoice(LightMoney money, string description, TimeSpan expiry);
    }
}
