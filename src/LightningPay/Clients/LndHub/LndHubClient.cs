using System;
using System.Net.Http;
using System.Threading.Tasks;

using LightningPay.Infrastructure.Api;

namespace LightningPay.Clients.LndHub
{
    public class LndHubClient : ApiServiceBase, ILightningClient
    {
        public LndHubClient(HttpClient client, 
            LndHubOptions options) : base(client,
            BuildAuthentication(options))
        {
        }


        public Task<LightningInvoice> CreateInvoice(LightMoney money, string description, TimeSpan expiry)
        {
            return null;
        }

        public Task<LightningInvoice> GetInvoice(string invoiceId)
        {
            throw new NotImplementedException();
        }

        private static AuthenticationBase BuildAuthentication(LndHubOptions options)
        {
            return new NoAuthentication();
        }
    }
}
