namespace LightningPay
{
    /// <summary>
    ///   Lightning event base class
    /// </summary>
    public abstract class LightningEvent
    {
        /// <summary>Gets or sets the error message.</summary>
        /// <value>The error message.</value>
        public string ErrorMessage { get; set; }
    }
}
