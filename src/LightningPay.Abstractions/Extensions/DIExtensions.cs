using System;
namespace LightningPay
{
    /// <summary>
    ///   Dependency injection extension methods
    /// </summary>
    public static class DIExtensions
    {
        /// <summary>Calls the event handler.</summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="handlerType">Type of the handler.</param>
        /// <param name="event">The event.</param>
        public static void CallEventHandler<TEvent>(this IServiceProvider serviceProvider, 
            Type handlerType,
            TEvent @event)
            where TEvent : LightningEvent
        {
            ILightningEventHandler<TEvent> handler = null;

            if (serviceProvider == null)
            {
                handler = Activator.CreateInstance(handlerType) as ILightningEventHandler<TEvent>;
            }
            else
            {
                handler = serviceProvider.GetService(handlerType) as ILightningEventHandler<TEvent>;
            }

            if (handler != null)
            {
                handler.Handle(@event);
            }
        }
    }
}
