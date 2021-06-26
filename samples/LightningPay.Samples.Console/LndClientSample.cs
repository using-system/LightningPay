using System;
using System.Threading.Tasks;

using LightningPay.Clients.Lnd;

namespace LightningPay.Samples.Console
{
    class LndClientSample : SampleBase
    {
        public async override Task Execute()
        {
            using (var lndClient = LndClient.New("http://localhost:42802/"))
            {
                var invoice = await lndClient.CreateInvoice(1, "Test", TimeSpan.FromMinutes(5));

                System.Console.WriteLine($"Create a new invoice with id {invoice.Id}");
                System.Console.WriteLine($"Payment request : {invoice.BOLT11}");
                System.Console.WriteLine($"Invoice Uri : {invoice.Uri}");

                while (! await lndClient.CheckPayment(invoice.Id))
                {
                    System.Console.WriteLine("Waiting for invoice payment....");
                    await Task.Delay(5000);
                }
            }

        }
    }
}
