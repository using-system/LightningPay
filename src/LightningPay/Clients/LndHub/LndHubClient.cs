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
    public class LndHubClient : ApiServiceBase, ILightningClient
    {
        protected readonly string address;

        private bool clientInternalBuilt = false;

        /// <summary>Initializes a new instance of the <see cref="LndHubClient" /> class.</summary>
        /// <param name="client">The client.</param>
        /// <param name="options">The options.</param>
        public LndHubClient(HttpClient client, 
            LndHubOptions options) : base(client,
            BuildAuthentication(options))
        {
            this.address = options.Address.ToBaseUrl();
        }

        /// <summary>Gets the wallet balance in satoshis.</summary>
        /// <returns>Balance is satoshis</returns>
        public async Task<long> GetBalance()
        {
            var response = await this.SendAsync<GetBalanceResponse>(HttpMethod.Get,
                $"{address}/balance");

            return response?.BTC?.AvailableBalance ?? 0;
        }


        /// <summary>Creates the invoice.</summary>
        /// <param name="satoshis">The amount in satoshis.</param>
        /// <param name="description">The description will be appears in the invoice.</param>
        /// <param name="options">Invoice creation options.</param>
        /// <returns>The lightning invoice just created</returns>
        /// <exception cref="LightningPay.ApiException">Cannot retrieve Payment request or request hash in the LNDHub api response</exception>
        public async Task<LightningInvoice> CreateInvoice(long satoshis, 
            string description, 
            CreateInvoiceOptions options = null)
        {
            var strAmount = satoshis.ToString(CultureInfo.InvariantCulture);
            var strExpiry = options.ToExpiryString();


            var request = new AddInvoiceRequest
            {
                Amount = strAmount,
                Memo = description,
                Expiry = strExpiry
            };

            var response = await this.SendAsync<AddInvoiceResponse>(HttpMethod.Post,
                $"{address}/addinvoice",
                request);

            if (string.IsNullOrEmpty(response.PaymentRequest)
                || response.R_hash == null)
            {
                throw new ApiException("Cannot retrieve Payment request or request hash in the LNDHub api response",
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
                $"{address}/checkpayment/{invoiceId}");

            return response.Paid;
        }

        /// <summary>Pay.</summary>
        /// <param name="paymentRequest">The payment request (aka bolt11).</param>
        /// <returns>True on the payment success, false otherwise</returns>
        public async Task<bool> Pay(string paymentRequest)
        {
            var response = await this.SendAsync<PayResponse>(HttpMethod.Post,
                $"{address}/payinvoice",
                new PayRequest() { PaymentRequest = paymentRequest });

            if (!string.IsNullOrEmpty(response.Error))
            {
                throw new ApiException($"Cannot proceed to the payment : {response.Error}",
                    System.Net.HttpStatusCode.BadRequest);
            }

            return true;
        }

        internal static AuthenticationBase BuildAuthentication(LndHubOptions options)
        {
            if(string.IsNullOrEmpty(options?.Login)
                || string.IsNullOrEmpty(options?.Password))
            {
                throw new ArgumentException("Login and Password are mandatory for lndhub authentication");
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
