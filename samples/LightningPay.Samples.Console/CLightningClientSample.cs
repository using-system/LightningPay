using System;
using System.Threading.Tasks;

using LightningPay.Clients.CLightning;


namespace LightningPay.Samples.Console
{
    class CLightningClientSample : SampleBase
    {
        public async override Task Execute()
        {
            using (CLightningClient client = CLightningClient.New("tcp://127.0.0.1:48532"))
            {
                System.Console.WriteLine($"check connectivity : {(await client.CheckConnectivity()).Result}");

                var balance = await client.GetBalance();
                System.Console.WriteLine($"Wallet balance : {balance.ToSatoshis()} sat ");

                var invoice = await client.CreateInvoice(Money.FromSatoshis(100),
                    "My First invoice", new CreateInvoiceOptions(expiry: TimeSpan.FromHours(12)));

                System.Console.WriteLine($"Create a new invoice with id {invoice.Id}");
                System.Console.WriteLine($"Payment request : {invoice.BOLT11}");
                System.Console.WriteLine($"Expiration date : {invoice.ExpiresAt}");
                System.Console.WriteLine($"Invoice Uri : {invoice.Uri}");

                while (!await client.CheckPayment(invoice.Id))
                {
                    System.Console.WriteLine("Waiting for invoice payment....");
                    await Task.Delay(5000);
                }
            }
        }
    }
}
