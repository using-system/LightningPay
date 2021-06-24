using System;
using System.Net.Http;
using System.Threading.Tasks;

using LightningPay.Infrastructure.Api;

namespace LightningPay.Clients.LndHub
{
    public class LndHubClient : ApiServiceBase, ILightningClient
    {
        private string baseUri;

        public LndHubClient(HttpClient client, 
            LndHubOptions options) : base(client,
            BuildAuthentication(options))
        {
            this.baseUri = options.BaseUri.ToBaseUrl();
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
            if(string.IsNullOrEmpty(options.Login)
                || string.IsNullOrEmpty(options.Password))
            {
                throw new ArgumentException("Login and Password are mandatory for lndhub authentication");
            }

            return new LndHubAuthentication(options);
        }
    }
}
