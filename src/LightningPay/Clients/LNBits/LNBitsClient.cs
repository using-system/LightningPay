using System;
using System.Threading.Tasks;
using System.Net.Http;

using LightningPay.Infrastructure.Api;

namespace LightningPay.Clients.LNBits
{
    /// <summary>
    ///   LNBits client
    /// </summary>
    public class LNBitsClient : ApiServiceBase, ILightningClient
    {
        private bool clientInternalBuilt = false;

        /// <summary>Initializes a new instance of the <see cref="LNBitsClient" /> class.</summary>
        /// <param name="client">The client.</param>
        /// <param name="options">The options.</param>
        public LNBitsClient(HttpClient client,
            LNBitsOptions options) : base(options.Address.ToBaseUrl(), client, BuildAuthentication(options))
        {

        }

        /// <summary>Gets the wallet balance in satoshis.</summary>
        /// <returns>Balance is satoshis</returns>
        public async Task<long> GetBalance()
        {
            var response = await this.SendAsync<GetWallletDetailsResponse>(HttpMethod.Get,
                 "api/v1/wallet");

            return response?.Balance / 1000 ?? 0;
        }

        /// <summary>Creates the invoice.</summary>
        /// <param name="satoshis">The amount in satoshis.</param>
        /// <param name="description">The description will be appears in the invoice.</param>
        /// <param name="options">Invoice creation options.</param>
        /// <returns>The lightning invoice just created</returns>
        /// <exception cref="LightningPay.ApiException">Cannot retrieve Payment request or request hash in the LNBits api response</exception>
        public async Task<LightningInvoice> CreateInvoice(long satoshis, string description, CreateInvoiceOptions options = null)
        {
            var request = new CreateInvoiceRequest
            {

                Out = false,
                Amount = satoshis,
                Memo = description
            };

            var response = await this.SendAsync<CreateInvoiceResponse>(HttpMethod.Post,
                "api/v1/payments",
                request);

            if (string.IsNullOrEmpty(response.PaymentRequest)
                || string.IsNullOrEmpty(response.PaymentHash))
            {
                throw new ApiException("Cannot retrieve Payment request or request hash in the LNBits api response",
                    System.Net.HttpStatusCode.BadRequest);
            }

            return response.ToLightningInvoice(satoshis, description, options);
        }

        /// <summary>Checks the payment of an invoice.</summary>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <returns>True of the invoice is paid, false otherwise</returns>
        public async Task<bool> CheckPayment(string invoiceId)
        {
            var response = await this.SendAsync<CheckPaymentResponse>(HttpMethod.Get,
                $"api/v1/payments/{invoiceId}");

            return response.Paid;
        }

        /// <summary>Pay.</summary>
        /// <param name="paymentRequest">The payment request (aka bolt11).</param>
        /// <returns>True on the payment success, false otherwise</returns>
        /// <exception cref="LightningPay.ApiException">Cannot proceed to the payment</exception>
        public async Task<bool> Pay(string paymentRequest)
        {
            var response = await this.SendAsync<PayResponse>(HttpMethod.Post,
                "api/v1/payments",
                new PayRequest() { Out = true, PaymentRequest = paymentRequest });

            if (string.IsNullOrEmpty(response.PaymentHash))
            {
                throw new ApiException($"Cannot proceed to the payment",
                    System.Net.HttpStatusCode.BadRequest);
            }

            return true;
        }

        internal static AuthenticationBase BuildAuthentication(LNBitsOptions options)
        {
            if(string.IsNullOrEmpty(options?.ApiKey))
            {
                throw new ArgumentException("Api key required for LNBits authentication");
            }

            return new LNBitsAuthentication(options.ApiKey);
        }

        /// <summary>>Instanciate a new LNBits api.</summary>
        /// <param name="address">The address.</param>
        /// <param name="apiKey">The API key.</param>
        /// <param name="httpClient">The HTTP client.</param>
        /// <returns>
        ///   Return the LNBits client
        /// </returns>
        public static LNBitsClient New(string address = "https://lnbits.com/",
            string apiKey = "",
            HttpClient httpClient = null)
        {
            bool clientInternalBuilt = false;

            if (httpClient == null)
            {
                httpClient = new HttpClient();
                clientInternalBuilt = true;
            }

            LNBitsClient client = new LNBitsClient(httpClient, new LNBitsOptions()
            {
                Address = new Uri(address),
                ApiKey = apiKey
            });

            client.clientInternalBuilt = clientInternalBuilt;

            return client;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            if (this.clientInternalBuilt)
            {
                this.httpClient?.Dispose();
            }
        }
    }
}
