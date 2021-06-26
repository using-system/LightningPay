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
                var lndHubClient = new LndHubClient(httpClient, new LndHubOptions()
                {
                    Address = new Uri("https://lndhub.herokuapp.com/"),
                    Login = "2073282b83fad2955b57",
                    Password = "a1c4f8c30a93bf3e8cbf"
                });

                var invoice = await lndHubClient.CreateInvoice(100, "Test", TimeSpan.FromMinutes(5));

                System.Console.WriteLine($"Create a new invoice with id {invoice.Id}");
                System.Console.WriteLine($"Payment request : {invoice.BOLT11}");
                System.Console.WriteLine($"Invoice Uri : {invoice.Uri}");

                while (true)
                {
                    System.Console.WriteLine("Waiting for invoice payment....");
                    await Task.Delay(5000);

                    bool isPaid = await lndHubClient.CheckPayment(invoice.Id);

                    if (isPaid)
                    {
                        break;
                    }
                }
            }
        }
    }
}
