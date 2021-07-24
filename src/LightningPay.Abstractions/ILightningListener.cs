﻿using System;
using System.Threading.Tasks;

namespace LightningPay
{
    /// <summary>
    ///   Lightning events listener
    /// </summary>
    public interface ILightningListener : IDisposable
    {
        /// <summary>Subscribes to an event.</summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        void Subscribe<TEvent, THandler>()
            where TEvent : LightningEvent
            where THandler : ILightningEventHandler<TEvent>;

        /// <summary>Subscribes the specified handler.</summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="handler">The handler.</param>
        void Subscribe<TEvent>(Action<TEvent> handler)
            where TEvent : LightningEvent;

        /// <summary>Unsubscribes to an event.</summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        void Unsubscribe<TEvent, THandler>()
          where TEvent : LightningEvent
          where THandler : ILightningEventHandler<TEvent>;

        /// <summary>Starts listening the events.</summary>
        Task StartListening();

        /// <summary>Stops listening the events.</summary>
        Task StopListening();
    }
}