using System.Threading.Tasks;

using LightningPay.Clients.Eclair;

namespace LightningPay.Samples.Console
{
    class EclairClientSample : SampleBase
    {
        public async override Task Execute()
        {
            using (var eclairClient = EclairClient.New("http://localhost:4570/", "eclairpassword"))
            {
                var invoice = await eclairClient.CreateInvoice(Money.FromSatoshis(100), 
                    "My First invoice");

                System.Console.WriteLine($"Create a new invoice with id {invoice.Id}");
                System.Console.WriteLine($"Payment request : {invoice.BOLT11}");
                System.Console.WriteLine($"Invoice Uri : {invoice.Uri}");

                while (!await eclairClient.CheckPayment(invoice.Id))
                {
                    System.Console.WriteLine("Waiting for invoice payment....");
                    await Task.Delay(5000);
                }
            }

        }
    }
}
