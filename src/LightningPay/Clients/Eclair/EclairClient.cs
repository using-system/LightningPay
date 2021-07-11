using System;
using System.Net.Http;
using System.Threading.Tasks;

using LightningPay.Infrastructure.Api;

namespace LightningPay.Clients.Eclair
{
    /// <summary>
    ///   Eclair client
    /// </summary>
    public class EclairClient : ApiServiceBase, IRestLightningClient
    {
        private bool clientInternalBuilt = false;

        /// <summary>Initializes a new instance of the <see cref="EclairClient" /> class.</summary>
        /// <param name="client">The client.</param>
        /// <param name="options">The options.</param>
        public EclairClient(HttpClient client,
           EclairOptions options) : base(options.Address.ToBaseUrl(), client, BuildAuthentication(options))
        {

        }

        /// <summary>Gets the wallet balance in satoshis.</summary>
        /// <returns>Balance is satoshis</returns>
        public Task<long> GetBalance()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>Creates the invoice.</summary>
        /// <param name="satoshis">The amount in satoshis.</param>
        /// <param name="description">The description will be appears in the invoice.</param>
        /// <param name="options">Invoice creation options.</param>
        /// <returns>The lightning invoice just created</returns>
        public Task<LightningInvoice> CreateInvoice(long satoshis, string description, CreateInvoiceOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>Checks the payment of an invoice.</summary>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <returns>True of the invoice is paid, false otherwise</returns>
        public Task<bool> CheckPayment(string invoiceId)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>Pay.</summary>
        /// <param name="paymentRequest">The payment request (aka bolt11).</param>
        /// <returns>True on the payment success, false otherwise</returns>
        public Task<bool> Pay(string paymentRequest)
        {
            throw new System.NotImplementedException();
        }

        internal static AuthenticationBase BuildAuthentication(EclairOptions options)
        {
            if (string.IsNullOrEmpty(options?.Password))
            {
                throw new ArgumentException("Password required for Eclair authentication");
            }

            return new EclairAuthentication(options.Password);
        }

        /// <summary>Instanciate a new Eclair client.</summary>
        /// <param name="address">The address.</param>
        /// <param name="password">The password.</param>
        /// <param name="httpClient">The HTTP client.</param>
        /// <returns>
        ///   Return the Eclair client
        /// </returns>
        public static EclairClient New(string address,
            string password = "",
            HttpClient httpClient = null)
        {
            bool clientInternalBuilt = false;

            if (httpClient == null)
            {
                httpClient = new HttpClient();
                clientInternalBuilt = true;
            }

            EclairClient client = new EclairClient(httpClient, new EclairOptions()
            {
                Address = new Uri(address),
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
