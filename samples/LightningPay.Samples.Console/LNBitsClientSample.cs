using System.Threading.Tasks;

using LightningPay.Clients.LNBits;


namespace LightningPay.Samples.Console
{
    class LNBitsClientSample : SampleBase
    {
        public async override Task Execute()
        {
            using (var lnBitsClient =
                LNBitsClient.New(apiKey: "YourApiKey"))
            {
                var balance = await lnBitsClient.GetBalance();
                System.Console.WriteLine($"Wallet balance : {balance} sat ");

                var invoice = await lnBitsClient.CreateInvoice(100, "My First invoice");

                System.Console.WriteLine($"Create a new invoice with id {invoice.Id}");
                System.Console.WriteLine($"Payment request : {invoice.BOLT11}");
                System.Console.WriteLine($"Invoice Uri : {invoice.Uri}");

                while (!await lnBitsClient.CheckPayment(invoice.Id))
                {
                    System.Console.WriteLine("Waiting for invoice payment....");
                    await Task.Delay(5000);
                }
            }
        }
    }
}
