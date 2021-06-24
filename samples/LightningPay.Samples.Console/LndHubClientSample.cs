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
                    Login = "2073282b83fad2955b57",
                    Password = "a1c4f8c30a93bf3e8cbf"
                });

                var invoice = await lndClient.CreateInvoice(LightMoney.Satoshis(100), "Test", TimeSpan.FromMinutes(5));

                System.Console.WriteLine($"Create a new invoice with id {invoice.Id}");
                System.Console.WriteLine($"Payment request : {invoice.BOLT11}");
                System.Console.WriteLine($"Invoice Uri : {invoice.Uri}");
            }
        }
    }
}
