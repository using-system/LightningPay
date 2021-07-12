namespace LightningPay
{
    /// <summary>
    ///   Pay Response
    /// </summary>
    public class PaymentResponse
    {
        /// <summary>Gets or sets the result.</summary>
        /// <value>The result.</value>
        public PayResult Result { get; set; }

        /// <summary>Gets or sets the error.</summary>
        /// <value>The error.</value>
        public string Error { get; set; }

        /// <summary>Initializes a new instance of the <see cref="PaymentResponse" /> class.</summary>
        /// <param name="result">The result.</param>
        public PaymentResponse(PayResult result)
        {
            Result = result;
        }
        /// <summary>Initializes a new instance of the <see cref="PaymentResponse" /> class.</summary>
        /// <param name="result">The result.</param>
        /// <param name="errorDetail">The error detail.</param>
        public PaymentResponse(PayResult result, string errorDetail)
        {
            this.Result = result;
            this.Error = errorDetail;
        }
    }
}
