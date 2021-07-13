using System;

namespace LightningPay
{
    /// <summary>
    ///   Money represent satoshis or milli satoshis
    /// </summary>
    public class Money : IComparable, IComparable<Money>, IEquatable<Money>
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
            return  (decimal)this.MilliSatoshis / (long) unit;
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

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="obj" /> in the sort order. Zero This instance occurs in the same position in the sort order as <paramref name="obj" />. Greater than zero This instance follows <paramref name="obj" /> in the sort order.
        /// </returns>
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            var money = obj as Money;

            return this.CompareTo(money);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="other" /> in the sort order.  Zero This instance occurs in the same position in the sort order as <paramref name="other" />. Greater than zero This instance follows <paramref name="other" /> in the sort order.
        /// </returns>
        public int CompareTo(Money other)
        {
            if (other == null)
                return 1;
            return this.MilliSatoshis.CompareTo(other.MilliSatoshis);
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///   <span class="keyword">
        ///     <span class="languageSpecificText">
        ///       <span class="cs">true</span>
        ///       <span class="vb">True</span>
        ///       <span class="cpp">true</span>
        ///     </span>
        ///   </span>
        ///   <span class="nu">
        ///     <span class="keyword">true</span> (<span class="keyword">True</span> in Visual Basic)</span> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <span class="keyword"><span class="languageSpecificText"><span class="cs">false</span><span class="vb">False</span><span class="cpp">false</span></span></span><span class="nu"><span class="keyword">false</span> (<span class="keyword">False</span> in Visual Basic)</span>.
        /// </returns>
        public bool Equals(Money other)
        {
            if (other == null)
                return false;
            return this.MilliSatoshis.Equals(other.MilliSatoshis);
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return this.MilliSatoshis.GetHashCode();
        }

        /// <summary>Determines whether the specified <see cref="System.Object" />, is equal to this instance.</summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            Money item = obj as Money;
            if (item == null)
                return false;
            return this.MilliSatoshis.Equals(item.MilliSatoshis);
        }

        #region Operators

        /// <summary>Implements the operator &lt;.</summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        /// <exception cref="System.ArgumentNullException">left
        /// or
        /// right</exception>
        public static bool operator <(Money left, Money right)
        {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");
            return left.MilliSatoshis < right.MilliSatoshis;
        }

        /// <summary>Implements the operator &gt;.</summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        /// <exception cref="System.ArgumentNullException">left
        /// or
        /// right</exception>
        public static bool operator >(Money left, Money right)
        {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");
            return left.MilliSatoshis > right.MilliSatoshis;
        }

        /// <summary>Implements the operator &lt;=.</summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        /// <exception cref="System.ArgumentNullException">left
        /// or
        /// right</exception>
        public static bool operator <=(Money left, Money right)
        {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");
            return left.MilliSatoshis <= right.MilliSatoshis;
        }

        /// <summary>Implements the operator &gt;=.</summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        /// <exception cref="System.ArgumentNullException">left
        /// or
        /// right</exception>
        public static bool operator >=(Money left, Money right)
        {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");
            return left.MilliSatoshis >= right.MilliSatoshis;
        }

        /// <summary>Implements the operator ==.</summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Money a, Money b)
        {
            if (Object.ReferenceEquals(a, b))
                return true;
            if (((object)a == null) || ((object)b == null))
                return false;
            return a.MilliSatoshis == b.MilliSatoshis;
        }

        /// <summary>Implements the operator !=.</summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Money a, Money b)
        {
            return !(a == b);
        }


        #endregion

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
