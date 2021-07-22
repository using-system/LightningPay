using System;

namespace LightningPay.Clients.Lnd
{
    public class LndListener : ILightningListener
    {
        public void Subscribe<TEvent, THandler>()
            where TEvent : LightningEvent
            where THandler : ILightningEventHandler<TEvent>
        {
            throw new NotImplementedException();
        }

        public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : LightningEvent
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe<TEvent, THandler>()
            where TEvent : LightningEvent
            where THandler : ILightningEventHandler<TEvent>
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
