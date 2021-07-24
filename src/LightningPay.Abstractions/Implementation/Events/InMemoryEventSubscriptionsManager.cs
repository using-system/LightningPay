using System;
using System.Linq;
using System.Collections.Generic;

namespace LightningPay
{
    /// <summary>
    ///   InMemory Event Subscriptions Manager
    /// </summary>
    public class InMemoryEventSubscriptionsManager : IEventSubscriptionsManager
    {
        private readonly Dictionary<string, List<Type>> handlers;

        /// <summary>Initializes a new instance of the <see cref="InMemoryEventSubscriptionsManager" /> class.</summary>
        public InMemoryEventSubscriptionsManager()
        {
            this.handlers = new Dictionary<string, List<Type>>();
        }

        /// <summary>Gets the event key.</summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <returns>Event key</returns>
        public string GetEventKey<TEvent>() where TEvent : LightningEvent
        {
            return typeof(TEvent).Name;
        }

        /// <summary>Adds the subscription.</summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        public void AddSubscription<TEvent, THandler>()
            where TEvent : LightningEvent
            where THandler : ILightningEventHandler<TEvent>
        {
            var eventName = this.GetEventKey<TEvent>();

            if(!this.handlers.ContainsKey(eventName))
            {
                handlers.Add(eventName, new List<Type>()
                {
                    typeof(THandler)
                });
            }
            else
            {
                handlers[eventName].Add(typeof(THandler));
            }
        }

        /// <summary>Removes the event subscription.</summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        public void RemoveEventSubscription<TEvent, THandler>()
            where TEvent : LightningEvent
            where THandler : ILightningEventHandler<TEvent>
        {
            var eventName = this.GetEventKey<TEvent>();

            if(this.handlers.ContainsKey(eventName))
            {
                handlers[eventName].RemoveAll((type) => typeof(THandler) == type);
            }
        }

        /// <summary>Gets the handlers for event.</summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <returns>Return all handler types corresponding the event</returns>
        public IEnumerable<Type> GetHandlersForEvent<TEvent>() where TEvent : LightningEvent
        {
            var eventName = this.GetEventKey<TEvent>();

            if (this.handlers.ContainsKey(eventName))
            {
                return this.handlers[eventName];
            }

            return Enumerable.Empty<Type>();
        }
    }
}
