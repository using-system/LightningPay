using LightningPay.Infrastructure.Api;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace LightningPay.Clients.Lnd
{
    public class LndClient : ApiServiceBase, ILightningClient
    {
        public string Address { get; private set; }

        private bool clientInternalBuilt = false;

        public LndClient(HttpClient client,
            LndOptions options) : base(client, BuildAuthentication(options))
        {
            this.Address = options.Address.ToBaseUrl();
        }

        public async Task<LightningInvoice> CreateInvoice(long satoshis, 
            string description, 
            CreateInvoiceOptions options = null)
        {
            var strAmount = satoshis.ToString(CultureInfo.InvariantCulture);
            var strExpiry = options.ToExpiryString();


            var request = new LnrpcInvoice
            {
                Value = strAmount,
                Memo = description,
                Expiry = strExpiry,
                Private = false
            };

            var response = await this.SendAsync<AddInvoiceResponse>(HttpMethod.Post,
                $"{Address}/v1/invoices",
                request);

            if(string.IsNullOrEmpty(response.Payment_request)
                || response.R_hash == null)
            {
                throw new ApiException("Cannot retrieve Payment request or request hash in the lnd api response", 
                    System.Net.HttpStatusCode.BadRequest);
            }

            return response.ToLightningInvoice(satoshis, description, options);
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
                $"{Address}/v1/invoice/{hash}");

            return response.ToLightningInvoice();
        }

        internal static AuthenticationBase BuildAuthentication(LndOptions options)
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
                Address = new Uri(address),
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
