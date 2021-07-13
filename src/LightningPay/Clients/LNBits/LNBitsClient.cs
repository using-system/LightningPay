using System;
using System.Threading.Tasks;
using System.Net.Http;

using LightningPay.Infrastructure.Api;

namespace LightningPay.Clients.LNBits
{
    /// <summary>
    ///   LNBits client
    /// </summary>
    public class LNBitsClient : ApiServiceBase, IRestLightningClient
    {
        private bool clientInternalBuilt = false;

        /// <summary>Initializes a new instance of the <see cref="LNBitsClient" /> class.</summary>
        /// <param name="client">The client.</param>
        /// <param name="options">The options.</param>
        public LNBitsClient(HttpClient client,
            LNBitsOptions options) : base(options.Address.ToBaseUrl(), client, BuildAuthentication(options))
        {

        }

        /// <summary>Checks the connectivity.</summary>
        public async Task<CheckConnectivityResponse> CheckConnectivity()
        {
            try
            {
                var response = await this.Get<GetWallletDetailsResponse>("api/v1/wallet");

                if (string.IsNullOrEmpty(response.Id))
                {
                    return new CheckConnectivityResponse(CheckConnectivityResult.Error, "Unable to retrieve the wallet id");
                }
            }
            catch (Exception exc)
            {
                return new CheckConnectivityResponse(CheckConnectivityResult.Error, exc.Message);
            }


            return new CheckConnectivityResponse(CheckConnectivityResult.Ok);
        }

        /// <summary>Gets the node / wallet balance.</summary>
        /// <returns>
        ///   Balance
        /// </returns>
        public async Task<Money> GetBalance()
        {
            var response = await this.Get<GetWallletDetailsResponse>("api/v1/wallet");

            var milliSatoshis =  response?.Balance ?? 0;

            return Money.FromMilliSatoshis(milliSatoshis);
        }

        /// <summary>Creates the invoice.</summary>
        /// <param name="satoshis">The amount in satoshis.</param>
        /// <param name="description">The description will be appears in the invoice.</param>
        /// <param name="options">Invoice creation options.</param>
        /// <returns>The lightning invoice just created</returns>
        /// <exception cref="LightningPay.LightningPayException">Cannot retrieve Payment request or request hash in the LNBits api response</exception>
        public async Task<LightningInvoice> CreateInvoice(long satoshis, string description, CreateInvoiceOptions options = null)
        {
            var request = new CreateInvoiceRequest
            {

                Out = false,
                Amount = satoshis,
                Memo = description
            };

            var response = await this.Post<CreateInvoiceResponse>("api/v1/payments", request);

            if (string.IsNullOrEmpty(response.PaymentRequest)
                || string.IsNullOrEmpty(response.PaymentHash))
            {
                throw new LightningPayException("Cannot retrieve Payment request or request hash in the LNBits api response",
                    LightningPayException.ErrorCode.BAD_REQUEST);
            }

            return response.ToLightningInvoice(satoshis, description, options);
        }

        /// <summary>Checks the payment of an invoice.</summary>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <returns>True of the invoice is paid, false otherwise</returns>
        public async Task<bool> CheckPayment(string invoiceId)
        {
            var response = await this.Get<CheckPaymentResponse>($"api/v1/payments/{invoiceId}");

            return response.Paid;
        }

        /// <summary>Pay.</summary>
        /// <param name="paymentRequest">The payment request (aka bolt11).</param>
        /// <returns>True on the payment success, false otherwise</returns>
        /// <returns>
        ///   PaymentResponse
        /// </returns>
        public async Task<PaymentResponse> Pay(string paymentRequest)
        {
            try
            {
                var response = await this.Post<PayResponse>("api/v1/payments",
                    new PayRequest() { Out = true, PaymentRequest = paymentRequest });

                if (string.IsNullOrEmpty(response.PaymentHash))
                {
                    throw new LightningPayException($"Cannot proceed to the payment",
                       LightningPayException.ErrorCode.UNAUTHORIZED);
                }
            }
            catch(LightningPayException exc)
            {
                return new PaymentResponse(PayResult.Error, exc.Message);
            }


            return new PaymentResponse(PayResult.Ok);
        }

        internal static AuthenticationBase BuildAuthentication(LNBitsOptions options)
        {
            if(string.IsNullOrEmpty(options?.ApiKey))
            {
                throw new ArgumentException("Api key required for LNBits authentication");
            }

            return new LNBitsAuthentication(options.ApiKey);
        }

        /// <summary>>Instanciate a new LNBits client.</summary>
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
