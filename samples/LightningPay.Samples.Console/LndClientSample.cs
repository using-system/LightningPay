using System;
using System.Net.Http;
using System.Threading.Tasks;


using LightningPay.Clients.Lnd;

namespace LightningPay.Samples.Console
{
    class LndClientSample : SampleBase
    {
        public async override Task Execute()
        {
            using(HttpClient httpClient = new HttpClient())
            {
                var lndClient = new LndClient(httpClient, new LndOptions()
                {
                    BaseUri = new Uri("http://localhost:42802/")
                });

                var invoice = await lndClient.CreateInvoice(LightMoney.Satoshis(1), "Test", TimeSpan.FromMinutes(5));

                System.Console.WriteLine(invoice.Id);
            }

        }
    }
}
