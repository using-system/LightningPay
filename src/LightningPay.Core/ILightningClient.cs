using System;
using System.Threading.Tasks;

namespace LightningPay.Core
{
    public interface ILightningClient
    {
        Task<LightningInvoice> CreateInvoice(LightMoney money, string description, TimeSpan expiry);
    }
}
