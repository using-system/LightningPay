using System;
using System.Collections.Generic;

namespace LightningPay
{
    /// <summary>
    ///   Event Subscriptions Manager interface
    /// </summary>
    public interface IEventSubscriptionsManager
    {
        /// <summary>Gets the event key.</summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <returns>
        ///   Event key
        /// </returns>
        string GetEventKey<TEvent>()
            where TEvent : LightningEvent;

        /// <summary>Adds the subscription.</summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        void AddSubscription<TEvent, THandler>()
           where TEvent : LightningEvent
           where THandler : ILightningEventHandler<TEvent>;

        /// <summary>Removes the event subscription.</summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        void RemoveEventSubscription<TEvent, THandler>()
          where TEvent : LightningEvent
          where THandler : ILightningEventHandler<TEvent>;

        /// <summary>Gets the handlers for event.</summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <returns>
        ///   Return all handler types corresponding the event
        /// </returns>
        IEnumerable<Type> GetHandlersForEvent<TEvent>() where TEvent : LightningEvent;
    }
}
