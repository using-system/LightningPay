﻿using System.Threading.Tasks;

using LightningPay.Clients.Lnd;

namespace LightningPay.Samples.Console
{
    class LndClientSample : SampleBase
    {
        public async override Task Execute()
        {
            using (var lndClient = LndClient.New("http://localhost:32736/"))
            {
                var balance = await lndClient.GetBalance();
                System.Console.WriteLine($"Wallet balance : {balance.ToSatoshis()} sat ");

                var invoice = await lndClient.CreateInvoice(Money.FromSatoshis(100), 
                    "My First invoice");

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
