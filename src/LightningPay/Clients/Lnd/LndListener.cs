using System;

namespace LightningPay.Clients.Lnd
{
    /// <summary>
    ///   LND events Listener
    /// </summary>
    public class LndListener : ILightningListener
    {
        private readonly IEventSubscriptionsManager eventSubscriptionsManager;

        /// <summary>Initializes a new instance of the <see cref="LndListener" /> class.</summary>
        /// <param name="options">The options.</param>
        /// <param name="eventSubscriptionsManager">The event subscriptions manager.</param>
        public LndListener(LndOptions options, IEventSubscriptionsManager eventSubscriptionsManager)
        {
            this.eventSubscriptionsManager = eventSubscriptionsManager;
        }

        /// <summary>Subscribes to an event.</summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Subscribe<TEvent, THandler>()
            where TEvent : LightningEvent
            where THandler : ILightningEventHandler<TEvent>
        {
            this.eventSubscriptionsManager.AddSubscription<TEvent, THandler>();
        }

        public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : LightningEvent
        {
            throw new NotImplementedException();
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

        /// <summary>Instanciate a new LND Listener.</summary>
        /// <param name="address">The address of the lnd api api.</param>
        /// <param name="macaroonHexString">The macaroon hexadecimal string.</param>
        /// <param name="macaroonBytes">The macaroon bytes.</param>
        /// <param name="eventSubscriptionsManager">Event subscription manager to use.</param>
        /// <returns>
        ///   LND Listener
        /// </returns>
        public static LndListener New(string address,
            string macaroonHexString = null,
            byte[] macaroonBytes = null,
            IEventSubscriptionsManager eventSubscriptionsManager = null)
        {

            if (eventSubscriptionsManager == null)
            {
                eventSubscriptionsManager = new InMemoryEventSubscriptionsManager();
            }

            if (!Uri.TryCreate(address, UriKind.Absolute, out Uri uri))
            {
                throw new LightningPayException($"Invalid uri format for LND Client : {address}",
                    LightningPayException.ErrorCode.BAD_CONFIGURATION);
            }

            LndListener listener = new LndListener(new LndOptions()
            {
                Address = new Uri(address),
                Macaroon = macaroonBytes ?? macaroonHexString.HexStringToByteArray()
            }, eventSubscriptionsManager);

            return listener;
        }


        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {

        }
    }
}
