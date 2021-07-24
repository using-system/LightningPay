using System.Threading.Tasks;

namespace LightningPay
{
    /// <summary>
    ///  Lightning Event Handler interface
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    public interface ILightningEventHandler<TEvent>
        where TEvent : LightningEvent
    {
        /// <summary>Handles the specified event.</summary>
        /// <param name="event">The event.</param>
        Task Handle(TEvent @event);
    }
}
