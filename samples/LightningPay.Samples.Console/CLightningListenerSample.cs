using System.Threading.Tasks;

using LightningPay.Clients.CLightning;

namespace LightningPay.Samples.Console
{
    class CLightningListenerSample : SampleBase
    {
        public async override Task Execute()
        {
            using (var listener = CLightningListener.New("tcp://127.0.0.1:48532"))
            {
                //Listen for invoices change
                listener.Subscribe<InvoiceUpdatedEvent, MyHandler>();
                await listener.StartListening();

                //Try to create a new invoice and check if the handlers are called
                using (var client = CLightningClient.New("tcp://127.0.0.1:48532"))
                {
                    var invoice = await client.CreateInvoice(Money.FromSatoshis(100), "My First invoice");
                }

                await Task.Delay(15000);
            }
        }

        public class MyHandler : ILightningEventHandler<InvoiceUpdatedEvent>
        {
            public Task Handle(InvoiceUpdatedEvent @event)
            {
                System.Console.WriteLine($"Handler 1 : Receive event {nameof(InvoiceUpdatedEvent)} for invoice {@event.Invoice.Id}");

                return Task.CompletedTask;
            }
        }
    }
}
