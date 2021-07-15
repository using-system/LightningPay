using System;
using System.Linq;
using System.Threading.Tasks;

namespace LightningPay.Clients.CLightning
{
    /// <summary>
    ///   C-Lightning client
    /// </summary>
    public class CLightningClient : ILightningClient
    {
		private readonly Uri address;

        private ICLightningTcpClient client;

        /// <summary>Initializes a new instance of the <see cref="CLightningClient" /> class.</summary>
        /// <param name="options">The options.</param>
        /// <param name="client">The client.</param>
        public CLightningClient(CLightningOptions options, ICLightningTcpClient client)
        {
            this.address = options.Address;
            this.client = client;
        }

        public async Task<CheckConnectivityResponse> CheckConnectivity()
        {
			try
			{
				var response = await client.SendCommandAsync<GetInfoResponse>(address, "getinfo");

				if (string.IsNullOrEmpty(response.Id))
				{
					return new CheckConnectivityResponse(CheckConnectivityResult.Error, "Unable to retrieve the node id");
				}
			}
			catch (Exception exc)
			{
				return new CheckConnectivityResponse(CheckConnectivityResult.Error, exc.Message);
			}

			return new CheckConnectivityResponse(CheckConnectivityResult.Ok);
		}

        /// <summary>Gets the node / wallet balance.</summary>
        /// <returns>Balance</returns>
        public Task<Money> GetBalance()
        {
            throw new System.NotImplementedException();
        }


        /// <summary>Creates the invoice.</summary>
        /// <param name="amount">The amount to receive.</param>
        /// <param name="description">The description will be appears in the invoice.</param>
        /// <param name="options">Invoice creation options.</param>
        /// <returns>The lightning invoice just created</returns>
        public Task<LightningInvoice> CreateInvoice(Money amount, string description, CreateInvoiceOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>Pay.</summary>
        /// <param name="paymentRequest">The payment request (aka bolt11).</param>
        /// <returns>PaymentResponse</returns>
        public Task<PaymentResponse> Pay(string paymentRequest)
        {
            throw new System.NotImplementedException();
        }


        /// <summary>Checks the payment of an invoice.</summary>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <returns>True of the invoice is paid, false otherwise</returns>
        public async Task<bool> CheckPayment(string invoiceId)
        {
            var invoice = await this.GetInvoice(invoiceId);

            if(invoice == null)
            {
                return false;
            }

            return invoice.Status == LightningInvoiceStatus.Paid;
        }

        private async Task<LightningInvoice> GetInvoice(string invoiceId)
        {
            var invoices = await this.client.SendCommandAsync<ListInvoicesResponse>(this.address,
                "listinvoices",
                new[] { invoiceId });

            return invoices.FirstOrDefault().ToLightningInvoice();
        }


        /// <summary>Instanciate a new C-Lightning client.</summary>
        /// <param name="address">The address of the C-Lightning server.</param>
        /// <returns>
        ///   Return the C-Lightning client
        /// </returns>
        public static CLightningClient New(string address)
        {
            return new CLightningClient(new CLightningOptions()
            {
                Address = new Uri(address)
            }, new DefaultCLightningTcpClient());
        }


        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {

        }
    }
}
