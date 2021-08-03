using System.Collections.Generic;

namespace LightningPay.Events.Eclair
{
    /// <summary>
    ///   Payment sent event on eclair node
    /// </summary>
    public class PaymentSentEvent : EclairEvent
    {
        /// <summary>Initializes a new instance of the <see cref="PaymentSentEvent" /> class.</summary>
        public PaymentSentEvent()
        {
            this.Parts = new List<PaymentSentPart>();
        }

        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        [Serializable("id")]
        public string Id { get; set; }

        /// <summary>Gets or sets the payment hash.</summary>
        /// <value>The payment hash.</value>
        [Serializable("paymentHash")]
        public string PaymentHash { get; set; }

        /// <summary>Gets or sets the payment preimage.</summary>
        /// <value>The payment preimage.</value>
        [Serializable("paymentPreimage")]
        public string PaymentPreimage { get; set; }

        /// <summary>Gets or sets the amount.</summary>
        /// <value>The amount.</value>
        [Serializable("recipientAmount")]
        public long Amount { get; set; }

        /// <summary>Gets or sets the parts.</summary>
        /// <value>The parts.</value>
        public List<PaymentSentPart> Parts { get; set; }

        /// <summary>
        ///   Payment part infos
        /// </summary>
        public class PaymentSentPart
        {
            /// <summary>Gets or sets the identifier.</summary>
            /// <value>The identifier.</value>
            [Serializable("id")]
            public string Id { get; set; }

            /// <summary>Gets or sets the amount.</summary>
            /// <value>The amount.</value>
            [Serializable("amount")]
            public long Amount { get; set; }

            /// <summary>Gets or sets the fees.</summary>
            /// <value>The fees.</value>
            [Serializable("feesPaid")]
            public long Fees { get; set; }

            /// <summary>Converts to channelid.</summary>
            /// <value>To channel identifier.</value>
            [Serializable("toChannelId")]
            public long ToChannelId { get; set; }

            /// <summary>Gets or sets the timestamp.</summary>
            /// <value>The timestamp.</value>
            [Serializable("timestamp")]
            public long Timestamp { get; set; }
        }
    }
}
