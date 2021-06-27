using System;
using System.Threading.Tasks;

using LightningPay.Clients.LndHub;

namespace LightningPay.Samples.Console
{
    class LndHubClientSample : SampleBase
    {
        public async override Task Execute()
        {
            using (var lndHubClient = 
                LndHubClient.New("https://lndhub.herokuapp.com/", "2073282b83fad2955b57", "a1c4f8c30a93bf3e8cbf"))
            {

                var invoice = await lndHubClient.CreateInvoice(100, "Test");

                System.Console.WriteLine($"Create a new invoice with id {invoice.Id}");
                System.Console.WriteLine($"Payment request : {invoice.BOLT11}");
                System.Console.WriteLine($"Invoice Uri : {invoice.Uri}");

                while (!await lndHubClient.CheckPayment(invoice.Id))
                {
                    System.Console.WriteLine("Waiting for invoice payment....");
                    await Task.Delay(5000);
                }
            }
        }
    }
}
