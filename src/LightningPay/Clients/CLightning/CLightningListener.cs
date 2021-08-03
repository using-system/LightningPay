using System;
using System.Threading;
using System.Threading.Tasks;

using LightningPay.Events.CLightning;

namespace LightningPay.Clients.CLightning
{
    /// <summary>
    ///   C-Lightning events listener
    /// </summary>
    public class CLightningListener : ILightningListener
    {
        private IRpcClient client;

        private readonly IEventSubscriptionsManager eventSubscriptionsManager;

        private readonly IServiceProvider serviceProvider;

        private long lastInvoiceIndex = 99999999999;

        private CancellationTokenSource cts = new CancellationTokenSource();

        private Task listenTask;

        /// <summary>Initializes a new instance of the <see cref="CLightningListener" /> class.</summary>
        /// <param name="client">The client.</param>
        /// <param name="eventSubscriptionsManager">The event subscriptions manager.</param>
        /// <param name="serviceProvider">The service provider.</param>
        public CLightningListener(IRpcClient client,
             IEventSubscriptionsManager eventSubscriptionsManager,
             IServiceProvider serviceProvider)
        {
            this.client = client;
            this.eventSubscriptionsManager = eventSubscriptionsManager;
            this.serviceProvider = serviceProvider;
        }

        /// <summary>Subscribes to an event.</summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        public void Subscribe<TEvent, THandler>()
            where TEvent : LightningEvent
            where THandler : ILightningEventHandler<TEvent>
        {
            this.eventSubscriptionsManager.AddSubscription<TEvent, THandler>();
        }

        /// <summary>Unsubscribes to an event.</summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        public void Unsubscribe<TEvent, THandler>()
            where TEvent : LightningEvent
            where THandler : ILightningEventHandler<TEvent>
        {
            this.eventSubscriptionsManager.RemoveEventSubscription<TEvent, THandler>();
        }

        /// <summary>Starts listening the events.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        public Task StartListening()
        {
            if (this.listenTask == null)
            {
                this.listenTask = this.ListenLoop();
            }

            return Task.CompletedTask;
        }

        private async Task ListenLoop()
        {
            while (!this.cts.IsCancellationRequested)
            {
                var invoice = await this.client.SendCommandAsync<CLightningInvoice>("waitanyinvoice", lastInvoiceIndex);

                foreach (var handlerType in this.eventSubscriptionsManager.GetHandlersForEvent<PaymentReceivedEvent>())
                {
                    this.serviceProvider.CallEventHandler(handlerType, invoice.ToEvent());
                }

                lastInvoiceIndex = invoice.PayIndex.Value;
            }
        }

        /// <summary>Stops listening the events.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        public Task StopListening()
        {
            this.cts.Cancel();

            return Task.CompletedTask;
        }

        /// <summary>Instanciate a new C-Lightning client.</summary>
        /// <param name="address">The address of the C-Lightning server.</param>
        /// <param name="eventSubscriptionsManager">Event subscription manager to use.</param>
        /// <returns>Return the C-Lightning listener</returns>
        public static CLightningListener New(string address,
            IEventSubscriptionsManager eventSubscriptionsManager = null)
        {
            if (!Uri.TryCreate(address, UriKind.Absolute, out Uri uri))
            {
                throw new LightningPayException($"Invalid uri format for C-Lightning Listener : {address}",
                    LightningPayException.ErrorCode.BAD_CONFIGURATION);
            }

            if (eventSubscriptionsManager == null)
            {
                eventSubscriptionsManager = new InMemoryEventSubscriptionsManager();
            }

            return new CLightningListener(new DefaultCLightningRpcClient(new CLightningOptions()
            {
                Address = new Uri(address)
            }), eventSubscriptionsManager, null);
        }


        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            if (!cts.IsCancellationRequested)
            {
                cts.Cancel();
            }
        }
    }
}
