using System;
using System.Globalization;
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


        public async Task<LightningInvoice> CreateInvoice(long satoshis, string description, TimeSpan expiry)
        {
            var strAmount = satoshis.ToString(CultureInfo.InvariantCulture);
            var strExpiry = Math.Round(expiry.TotalSeconds, 0).ToString(CultureInfo.InvariantCulture);


            var request = new AddInvoiceRequest
            {
                Amount = strAmount,
                Memo = description,
                Expiry = strExpiry
            };

            var response = await this.SendAsync<AddInvoiceResponse>(HttpMethod.Post,
                $"{baseUri}/addinvoice",
                request);

            return response.ToLightningInvoice(satoshis, description, expiry);
        }

        public async Task<bool> CheckPayment(string invoiceId)
        {
            var response = await this.SendAsync<CheckPaymentResponse>(HttpMethod.Get,
                $"{baseUri}/checkpayment/{invoiceId}");

            return response.Paid;
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
