using System;
using System.Net.Http;
using System.Threading.Tasks;

using LightningPay.Clients.LndHub;

namespace LightningPay.Samples.Console
{
    class LndHubClientSample : SampleBase
    {
        public async override Task Execute()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var lndClient = new LndHubClient(httpClient, new LndHubOptions()
                {
                    BaseUri = new Uri("https://lndhub.herokuapp.com/"),
                    Login = "",
                    Password = ""
                });

                var invoice = await lndClient.CreateInvoice(LightMoney.Satoshis(100), "Test", TimeSpan.FromMinutes(5));
            }
        }
    }
}
