using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

using LightningPay.Infrastructure.Api;

namespace LightningPay.Clients.LndHub
{
    /// <summary>
    ///   LNDHub client
    /// </summary>
    public class LndHubClient : ApiServiceBase, IRestLightningClient
    {
        private bool clientInternalBuilt = false;

        /// <summary>Initializes a new instance of the <see cref="LndHubClient" /> class.</summary>
        /// <param name="client">The client.</param>
        /// <param name="options">The options.</param>
        public LndHubClient(HttpClient client, 
            LndHubOptions options) : base(options.Address.ToBaseUrl(), client, BuildAuthentication(options))
        {
        }

        /// <summary>Checks the connectivity.</summary>
        public async Task<CheckConnectivityResponse> CheckConnectivity()
        {
            try
            {
                var response = await this.Get<GetInfoResponse>("getinfo");

                if (string.IsNullOrEmpty(response.Alias))
                {
                    return new CheckConnectivityResponse(CheckConnectivityResult.Error, "Unable to retrieve the node alias");
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
            var response = await this.Get<GetBalanceResponse>("balance");

            var satoshis = response?.BTC?.AvailableBalance ?? 0;

            return Money.FromSatoshis(satoshis);
        }


        /// <summary>Creates the invoice.</summary>
        /// <param name="amount">The amount to receive.</param>
        /// <param name="description">The description will be appears in the invoice.</param>
        /// <param name="options">Invoice creation options.</param>
        /// <returns>The lightning invoice just created</returns>
        /// <exception cref="LightningPay.LightningPayException">Cannot retrieve Payment request or request hash in the LNDHub api response</exception>
        public async Task<LightningInvoice> CreateInvoice(Money amount, 
            string description, 
            CreateInvoiceOptions options = null)
        {
            var strAmount = ((long)amount.ToSatoshis()).ToString(CultureInfo.InvariantCulture);
            var strExpiry = options.ToExpiryString();


            var request = new AddInvoiceRequest
            {
                Amount = strAmount,
                Memo = description,
                Expiry = strExpiry
            };

            var response = await this.Post<AddInvoiceResponse>("addinvoice",
                request);
            this.CheckResponse(response);

            if (string.IsNullOrEmpty(response.PaymentRequest)
                || response.R_hash == null)
            {
                throw new LightningPayException("Cannot retrieve Payment request or request hash in the LNDHub api response",
                    LightningPayException.ErrorCode.BAD_REQUEST);
            }

            return response.ToLightningInvoice(amount, description, options);
        }

        /// <summary>Checks the payment of an invoice.</summary>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <returns>True of the invoice is paid, false otherwise</returns>
        public async Task<bool> CheckPayment(string invoiceId)
        {
            var response = await this.Get<CheckPaymentResponse>($"checkpayment/{invoiceId}");
            this.CheckResponse(response);

            return response.Paid;
        }

        /// <summary>Pay.</summary>
        /// <param name="paymentRequest">The payment request (aka bolt11).</param>
        /// <returns>
        ///   PaymentResponse
        /// </returns>
        public async Task<PaymentResponse> Pay(string paymentRequest)
        {
            try
            {
                var response = await this.Post<PayResponse>("payinvoice",
                    new PayRequest() { PaymentRequest = paymentRequest });
                this.CheckResponse(response);

                if (!string.IsNullOrEmpty(response.Error))
                {
                    throw new LightningPayException($"Cannot proceed to the payment : {response.Error}",
                        LightningPayException.ErrorCode.BAD_REQUEST);
                }
            }
            catch (LightningPayException exc)
            {
                return new PaymentResponse(PayResult.Error, exc.Message);
            }


            return new PaymentResponse(PayResult.Ok);

        }

        internal static AuthenticationBase BuildAuthentication(LndHubOptions options)
        {
            if(string.IsNullOrEmpty(options?.Login)
                || string.IsNullOrEmpty(options?.Password))
            {
                return new NoAuthentication();
            }

            return new LndHubAuthentication(options);
        }

        /// <summary>Instanciate a new LNDHub client.</summary>
        /// <param name="address">The address of the LNDHub api.</param>
        /// <param name="login">The login.</param>
        /// <param name="password">The password.</param>
        /// <param name="httpClient">The HTTP client.</param>
        /// <returns>
        ///   Return the LNDHub client
        /// </returns>
        public static LndHubClient New(string address = "https://lndhub.herokuapp.com/",
           string login = "",
           string password = "",
           HttpClient httpClient = null)
        {
            bool clientInternalBuilt = false;

            if (httpClient == null)
            {
                httpClient = new HttpClient();
                clientInternalBuilt = true;
            }

            LndHubClient client = new LndHubClient(httpClient, new LndHubOptions()
            {
                Address = new Uri(address),
                Login = login,
                Password = password
            });

            client.clientInternalBuilt = clientInternalBuilt;

            return client;
        }

        private void CheckResponse(ResponseBase response)
        {
            if(response.Failed)
            {
                throw new LightningPayException(response.Message,
                    LightningPayException.ErrorCode.BAD_REQUEST);
            }
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
