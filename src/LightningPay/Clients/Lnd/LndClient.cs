using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

using LightningPay.Infrastructure.Api;

namespace LightningPay.Clients.Lnd
{
    /// <summary>
    ///   LND Client
    /// </summary>
    public class LndClient : ApiServiceBase, ILightningClient
    {
        private readonly string address;

        private bool clientInternalBuilt = false;

        /// <summary>Initializes a new instance of the <see cref="LndClient" /> class.</summary>
        /// <param name="client">The client.</param>
        /// <param name="options">The options.</param>
        public LndClient(HttpClient client,
            LndOptions options) : base(client, BuildAuthentication(options))
        {
            this.address = options.Address.ToBaseUrl();
        }

        /// <summary>Gets the wallet balance in satoshis.</summary>
        /// <returns>Balance is satoshis</returns>
        public async Task<long> GetBalance()
        {
            var response = await this.SendAsync<GetBalanceResponse>(HttpMethod.Get,
                $"{address}/v1/balance/blockchain");

            return response?.TotalBalance ?? 0;
        }

        /// <summary>Creates the invoice.</summary>
        /// <param name="satoshis">The amount in satoshis.</param>
        /// <param name="description">The description will be appears in the invoice.</param>
        /// <param name="options">Invoice creation options.</param>
        /// <returns>The lightning invoice just created</returns>
        /// <exception cref="LightningPay.ApiException">Cannot retrieve Payment request or request hash in the lnd api response</exception>
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
                $"{address}/v1/invoices",
                request);

            if(string.IsNullOrEmpty(response.Payment_request)
                || response.R_hash == null)
            {
                throw new ApiException("Cannot retrieve Payment request or request hash in the lnd api response", 
                    System.Net.HttpStatusCode.BadRequest);
            }

            return response.ToLightningInvoice(satoshis, description, options);
        }

        /// <summary>Checks the payment of an invoice.</summary>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <returns>True of the invoice is paid, false otherwise</returns>
        public async Task<bool> CheckPayment(string invoiceId)
        {
            var invoice = await this.GetInvoice(invoiceId);

            return invoice.Status == LightningInvoiceStatus.Paid;
        }

        private async Task<LightningInvoice> GetInvoice(string invoiceId)
        {
            var hash = Uri.EscapeDataString(Convert.ToString(invoiceId, CultureInfo.InvariantCulture));
            var response = await this.SendAsync<LnrpcInvoice>(HttpMethod.Get,
                $"{address}/v1/invoice/{hash}");

            return response.ToLightningInvoice();
        }

        /// <summary>Pay.</summary>
        /// <param name="paymentRequest">The payment request (aka bolt11).</param>
        /// <returns>True on the payment success, false otherwise</returns>
        public async Task<bool> Pay(string paymentRequest)
        {
            var response = await this.SendAsync<PayResponse>(HttpMethod.Post,
                $"{address}/v1/channels/transactions",
                new PayRequest() { PaymentRequest = paymentRequest });

            if(!string.IsNullOrEmpty(response.Error))
            {
                throw new ApiException($"Cannot proceed to the payment : {response.Error}",
                    System.Net.HttpStatusCode.BadRequest);
            }

            return true;
        }

        internal static AuthenticationBase BuildAuthentication(LndOptions options)
        {
            if(options.Macaroon != null)
            {
                return new MacaroonAuthentication(options.Macaroon);
            }

            return new NoAuthentication();
        }

        /// <summary>Instanciate a new LND Client.</summary>
        /// <param name="address">The address of the lnd api server.</param>
        /// <param name="macaroonHexString">The macaroon hexadecimal string.</param>
        /// <param name="macaroonBytes">The macaroon bytes.</param>
        /// <param name="httpClient">The HTTP client.</param>
        /// <returns>
        ///   LND Client
        /// </returns>
        public static LndClient New(string address, 
            string macaroonHexString = null,
            byte[] macaroonBytes = null,
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
                Macaroon = macaroonBytes?? macaroonHexString.HexStringToByteArray()
            });

            client.clientInternalBuilt = clientInternalBuilt;

            return client;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            if(this.clientInternalBuilt)
            {
                this.httpClient?.Dispose();
            }
        }

    }
}
