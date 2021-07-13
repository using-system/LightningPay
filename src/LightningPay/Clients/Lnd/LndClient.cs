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
    public class LndClient : ApiServiceBase, IRestLightningClient
    {
        private bool clientInternalBuilt = false;

        /// <summary>Initializes a new instance of the <see cref="LndClient" /> class.</summary>
        /// <param name="client">The client.</param>
        /// <param name="options">The options.</param>
        public LndClient(HttpClient client,
            LndOptions options) : base(options.Address.ToBaseUrl(), client, BuildAuthentication(options))
        {
        }

        /// <summary>Checks the connectivity.</summary>
        public async Task<CheckConnectivityResponse> CheckConnectivity()
        {
            try
            {
                var response = await this.Get<GetInfoResponse>("/v1/getinfo");

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
            var response = await this.Get<GetBalanceResponse>("v1/balance/blockchain");

            var satoshis = response?.TotalBalance ?? 0;

            return Money.FromSatoshis(satoshis);
        }

        /// <summary>Creates the invoice.</summary>
        /// <param name="amount">The amount to receive.</param>
        /// <param name="description">The description will be appears in the invoice.</param>
        /// <param name="options">Invoice creation options.</param>
        /// <returns>The lightning invoice just created</returns>
        /// <exception cref="LightningPay.LightningPayException">Cannot retrieve Payment request or request hash in the lnd api response</exception>
        public async Task<LightningInvoice> CreateInvoice(Money amount, 
            string description, 
            CreateInvoiceOptions options = null)
        {
            var strAmount = ((long)amount.ToSatoshis()).ToString(CultureInfo.InvariantCulture);
            var strExpiry = options.ToExpiryString();


            var request = new LnrpcInvoice
            {
                Value = strAmount,
                Memo = description,
                Expiry = strExpiry,
                Private = false
            };

            var response = await this.Post<AddInvoiceResponse>("v1/invoices",
                request);

            if(string.IsNullOrEmpty(response.Payment_request)
                || response.R_hash == null)
            {
                throw new LightningPayException("Cannot retrieve Payment request or request hash in the lnd api response",
                    LightningPayException.ErrorCode.BAD_REQUEST);
            }

            return response.ToLightningInvoice(amount, description, options);
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
            var response = await this.Get<LnrpcInvoice>($"v1/invoice/{hash}");

            return response.ToLightningInvoice();
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
                var response = await this.Post<PayResponse>("v1/channels/transactions",
                    new PayRequest() { PaymentRequest = paymentRequest });

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

        internal static AuthenticationBase BuildAuthentication(LndOptions options)
        {
            if(options.Macaroon != null)
            {
                return new MacaroonAuthentication(options.Macaroon);
            }

            return new NoAuthentication();
        }

        /// <summary>Instanciate a new LND Client.</summary>
        /// <param name="address">The address of the lnd api api.</param>
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
