using System;
using System.Linq;
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

        /// <summary>Checks the connectivity.</summary>
        /// <returns>True of the connectivity is ok, false otherwise</returns>
        public async Task<CheckConnectivityResponse> CheckConnectivity()
        {
            try
            {
                var response = await this.Post<GetInfoResponse>("getinfo", formUrlEncoded: true);

                if (string.IsNullOrEmpty(response.NodeId))
                {
                    return new CheckConnectivityResponse(CheckConnectivityResult.Error, "Unable to retrieve the node id");
                }
            }
            catch(Exception exc)
            {
                return new CheckConnectivityResponse(CheckConnectivityResult.Error, exc.Message);
            }

            return new CheckConnectivityResponse(CheckConnectivityResult.Ok);
        }

        /// <summary>Gets the wallet balance in satoshis.</summary>
        /// <returns>Balance is satoshis</returns>
        public async Task<long> GetBalance()
        {
            var response = await this.Post<GetBalanceResponse>("onchainbalance");

            return response?.Confirmed ?? 0;
            throw new NotImplementedException("Eclair client does not support GetBlance");
        }

        /// <summary>Creates the invoice.</summary>
        /// <param name="satoshis">The amount in satoshis.</param>
        /// <param name="description">The description will be appears in the invoice.</param>
        /// <param name="options">Invoice creation options.</param>
        /// <returns>The lightning invoice just created</returns>
        public async Task<LightningInvoice> CreateInvoice(long satoshis, string description, CreateInvoiceOptions options = null)
        {
            var response = await this.Post<CreateInvoiceResponse>("createinvoice",
                new CreateInvoiceRequest()
                {
                    Description = description,
                    AmountMsat = satoshis * 1000,
                    ExpireIn = int.Parse(options.ToExpiryString())
                }, formUrlEncoded: true);

            return response.ToLightningInvoice();
        }

        /// <summary>Checks the payment of an invoice.</summary>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <returns>True of the invoice is paid, false otherwise</returns>
        public async Task<bool> CheckPayment(string invoiceId)
        {
            var response = await this.Post<GetReceivedInfoResponse>("getreceivedinfo",
                new GetReceivedInfoRequest()
                {
                    PaymentHash = invoiceId
                }, formUrlEncoded: true);

            return response?.Status?.Type == "received"
                && response?.Amount > 0
                && response.Amount == response.Status.Amount;
        }

        /// <summary>Pay.</summary>
        /// <param name="paymentRequest">The payment request (aka bolt11).</param>
        /// <returns>
        ///   PaymentResponse
        /// </returns>
        public async Task<PaymentResponse> Pay(string paymentRequest)
        {
            string id  = await this.Post<string>("payinvoice",
                new PayRequest()
                {
                    PaymentRequest = paymentRequest
                }, formUrlEncoded: true);

            if (string.IsNullOrEmpty(id))
            {
                throw new LightningPayException("Cannot proceed to the payment",
                    LightningPayException.ErrorCode.BAD_REQUEST);
            }

            while(true)
            {
                var paymentResponse = await this.Post<GetSentInfoResponse>("getsentinfo",
                    new GetSentInfoRequest()
                    {
                        Id = id
                    }, formUrlEncoded: true);
                if(paymentResponse?.Any() == false)
                {
                    continue;
                }
                var status = paymentResponse.FirstOrDefault().Status?.Type;
                switch(status)
                {
                    case "sent":
                        return new PaymentResponse(PayResult.Ok);
                    case "failed":
                        return new PaymentResponse(PayResult.Error, "Cannot process to the payment");
                    case "pending":
                        await Task.Delay(200);
                        break;
                    default:
                        return new PaymentResponse(PayResult.Error, "Cannot process to the payment");
                }
            }
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
