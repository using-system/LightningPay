using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

using LightningPay.Infrastructure.Api;

namespace LightningPay.Clients.Lnd
{
    public class LndClient : ApiServiceBase, ILightningClient
    {
        private string baseUri;

        private readonly HttpClient client;

        public LndClient(HttpClient client,
            IOptions<LndOptions> options) : base(client)
        {
            this.client = client;
            this.baseUri = options.Value.BaseUri.ToString().TrimEnd('/');
        }

        public async Task<LightningInvoice> CreateInvoice(LightMoney money, string description, TimeSpan expiry)
        {
            var strAmount = money.ToUnit(LightMoneyUnit.Satoshi).ToString(CultureInfo.InvariantCulture);
            var strExpiry = Math.Round(expiry.TotalSeconds, 0).ToString(CultureInfo.InvariantCulture);


            var request = new AddInvoiceRequest
            {
                Value = strAmount,
                Memo = description,
                Expiry = strExpiry,
                Private = false
            };

            var response = await this.SendAsync<AddInvoiceResponse>(HttpMethod.Post,
                $"{baseUri}/v1/invoices",
                request);


            var invoice = new LightningInvoice
            {
                Id = BitConverter.ToString(response.R_hash)
                  .Replace("-", "")
                  .ToLower(CultureInfo.InvariantCulture),
                Amount = money,
                Status = LightningInvoiceStatus.Unpaid,
                ExpiresAt = DateTimeOffset.UtcNow + expiry
            };

            return invoice;
        }
    }
}
