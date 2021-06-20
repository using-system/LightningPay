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
            LndOptions options) : base(client)
        {
            this.baseUri = options.BaseUri.ToString().TrimEnd('/');
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
