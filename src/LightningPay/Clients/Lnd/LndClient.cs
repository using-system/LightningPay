using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

using LightningPay.Infrastructure.Api;

namespace LightningPay.Clients.Lnd
{
    public class LndClient : ApiServiceBase, ILightningClient
    {
        private string baseUri;

        public LndClient(HttpClient client,
            LndOptions options) : base(client, BuildAuthentication(options))
        {
            this.baseUri = options.BaseUri.ToBaseUrl();
        }

        public async Task<LightningInvoice> CreateInvoice(LightMoney money, string description, TimeSpan expiry)
        {
            var strAmount = money.ToUnit(LightMoneyUnit.Satoshi).ToString(CultureInfo.InvariantCulture);
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

            return response.ToLightningInvoice(money, description, expiry);
        }

        public async Task<LightningInvoice> GetInvoice(string invoiceId)
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
    }
}
