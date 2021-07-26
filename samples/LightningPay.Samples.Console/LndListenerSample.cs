using System.Threading.Tasks;

using LightningPay.Clients.Lnd;

namespace LightningPay.Samples.Console
{
    class LndListenerSample : SampleBase
    {
        public async override Task Execute()
        {
            using (var listener = LndListener.New("http://localhost:32736/"))
            {
                //Listen for invoices change
                listener.Subscribe<InvoiceUpdatedEvent, MyHandler>();
                listener.Subscribe<InvoiceUpdatedEvent>((@event) =>
                {
                    System.Console.WriteLine($"Handler 2 : Receive event {nameof(InvoiceUpdatedEvent)} for invoice {@event.Invoice.Id}");
                });
                await listener.StartListening();

                //Try to create a new invoice and check if the handlers are called
                using (var lndClient = LndClient.New("http://localhost:32736/"))
                {
                    var invoice = await lndClient.CreateInvoice(Money.FromSatoshis(100), "My First invoice");
                }
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
