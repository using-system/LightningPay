namespace LightningPay.Events.Eclair
{
    /// <summary>
    ///  Eclair event base class
    /// </summary>
    public abstract class EclairEvent : LightningEvent
    {
        /// <summary>Gets or sets the evebt type.</summary>
        /// <value>The event type.</value>
        [Serializable("type")]
        public string Type { get; set; }
    }
}
