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

                System.Console.WriteLine($"Create a new invoice with id {invoice.Id}");
                System.Console.WriteLine($"Payment request : {invoice.BOLT11}");
                System.Console.WriteLine($"Invoice Uri : {invoice.Uri}");

                while (true)
                {
                    System.Console.WriteLine("Waiting for invoice payment....");
                    await Task.Delay(5000);

                    bool isPaid = await lndClient.CheckPayment(invoice.Id);

                    if(isPaid)
                    {
                        break;
                    }
                }
            }

        }
    }
}
