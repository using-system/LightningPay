namespace LightningPay
{
    /// <summary>
    ///   Money represent satoshis or milli satoshis
    /// </summary>
    public class Money
    {
        /// <summary>Gets the amount.</summary>
        /// <value>The amount.</value>
        public long MilliSatoshis { get; private set; }

        /// <summary>Initializes a new instance of the <see cref="Money" /> class.</summary>
        /// <param name="amount">The amount.</param>
        /// <param name="unit">The unit.</param>
        public Money(decimal amount, MoneyUnit unit)
        {
            this.MilliSatoshis = (long) amount * (long) unit;
        }

        /// <summary>Converts with the specified unit.</summary>
        /// <param name="unit">The unit.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public decimal ToUnit(MoneyUnit unit)
        {
            return this.MilliSatoshis / (long) unit;
        }

        /// <summary>Converts to satoshis.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        public decimal ToSatoshis()
        {
            return this.ToUnit(MoneyUnit.Satoshi);
        }

        /// <summary>Converts to btc.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        public decimal ToBTC()
        {
            return this.ToUnit(MoneyUnit.BTC);
        }

        /// <summary>Create a new money instance from satoshis.</summary>
        /// <param name="sats">The sats.</param>
        /// <returns>
        ///   Money
        /// </returns>
        public static Money FromSatoshis(long sats)
        {
            return new Money(sats, MoneyUnit.Satoshi);
        }

        /// <summary>Create a new money instance from milli satoshis.</summary>
        /// <param name="msats">The msats.</param>
        /// <returns>
        ///   Money
        /// </returns>
        public static Money FromMilliSatoshis(long msats)
        {
            return new Money(msats, MoneyUnit.MilliSatoshi);
        }
    }
}
