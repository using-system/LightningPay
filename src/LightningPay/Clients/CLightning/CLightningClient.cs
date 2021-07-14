using System;
using System.Threading.Tasks;

namespace LightningPay.Clients.CLightning
{
    public class CLightningClient : ILightningClient
    {
		private readonly Uri address;

        private ICLightningTcpClient client;

		public CLightningClient(Uri address, ICLightningTcpClient client)
        {
			this.address = address;
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

        public Task<Money> GetBalance()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> CheckPayment(string invoiceId)
        {
            throw new System.NotImplementedException();
        }

        public Task<LightningInvoice> CreateInvoice(Money amount, string description, CreateInvoiceOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<PaymentResponse> Pay(string paymentRequest)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {

        }
	}
}
