using System.Threading.Tasks;

using LightningPay.Clients.LndHub;

namespace LightningPay.Samples.Console
{
    class LndHubClientSample : SampleBase
    {
        public async override Task Execute()
        {
            using (var lndHubClient = 
                LndHubClient.New(login: "2073282b83fad2955b57", password: "a1c4f8c30a93bf3e8cbf"))
            {
                var balance = await lndHubClient.GetBalance();
                System.Console.WriteLine($"Wallet balance : {balance.ToSatoshis()} sat ");

                var invoice = await lndHubClient.CreateInvoice(Money.FromSatoshis(100), 
                    "My First invoice");

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
