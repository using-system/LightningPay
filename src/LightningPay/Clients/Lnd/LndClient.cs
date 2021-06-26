using LightningPay.Infrastructure.Api;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace LightningPay.Clients.Lnd
{
    public class LndClient : ApiServiceBase, ILightningClient
    {
        private string baseUri;

        private bool clientInternalBuilt = false;

        public LndClient(HttpClient client,
            LndOptions options) : base(client, BuildAuthentication(options))
        {
            this.baseUri = options.BaseUri.ToBaseUrl();
        }

        public async Task<LightningInvoice> CreateInvoice(long satoshis, string description, TimeSpan expiry)
        {
            var strAmount = satoshis.ToString(CultureInfo.InvariantCulture);
            var strExpiry = Math.Round(expiry.TotalSeconds, 0).ToString(CultureInfo.InvariantCulture);


            var request = new LnrpcInvoice
            {
                Value = strAmount,
                Memo = description,
                Expiry = strExpiry,
                Private = false
            };

            var response = await this.SendAsync<AddInvoiceResponse>(HttpMethod.Post,
                $"{baseUri}/v1/invoices",
                request);

            return response.ToLightningInvoice(satoshis, description, expiry);
        }

        public async Task<bool> CheckPayment(string invoiceId)
        {
            var invoice = await this.GetInvoice(invoiceId);

            return invoice.Status == LightningInvoiceStatus.Paid;
        }

        private async Task<LightningInvoice> GetInvoice(string invoiceId)
        {
            var hash = Uri.EscapeDataString(Convert.ToString(invoiceId, CultureInfo.InvariantCulture));
            var response = await this.SendAsync<LnrpcInvoice>(HttpMethod.Get,
                $"{baseUri}/v1/invoice/{hash}");

            return response.ToLightningInvoice();
        }

        private static AuthenticationBase BuildAuthentication(LndOptions options)
        {
            if(options.Macaroon != null)
            {
                return new MacaroonAuthentication(options.Macaroon);
            }

            return new NoAuthentication();
        }

        public static LndClient New(string address, 
            byte[] macaroon = null,
            HttpClient httpClient = null)
        {
            bool clientInternalBuilt = false;

            if(httpClient == null)
            {
                httpClient = new HttpClient();
                clientInternalBuilt = true;
            }

            LndClient client = new LndClient(httpClient, new LndOptions()
            {
                BaseUri = new Uri(address),
                Macaroon = macaroon
            });

            client.clientInternalBuilt = clientInternalBuilt;

            return client;
        }

        public void Dispose()
        {
            if(this.clientInternalBuilt)
            {
                this.httpClient?.Dispose();
            }
        }
    }
}
