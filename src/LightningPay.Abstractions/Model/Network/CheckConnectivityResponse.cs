namespace LightningPay
{
    /// <summary>
    /// Check Connectivity Response
    /// </summary>
    public class CheckConnectivityResponse
    {
        /// <summary>Gets or sets the result.</summary>
        /// <value>The result.</value>
        public CheckConnectivityResult Result { get; set; }

        /// <summary>Gets or sets the error.</summary>
        /// <value>The error.</value>
        public string Error { get; set; }

        /// <summary>Initializes a new instance of the <see cref="CheckConnectivityResponse" /> class.</summary>
        /// <param name="result">The result.</param>
        public CheckConnectivityResponse(CheckConnectivityResult result)
        {
            Result = result;
        }

        /// <summary>Initializes a new instance of the <see cref="CheckConnectivityResponse" /> class.</summary>
        /// <param name="result">The result.</param>
        /// <param name="errorDetail">The error detail.</param>
        public CheckConnectivityResponse(CheckConnectivityResult result, string errorDetail)
        {
            this.Result = result;
            this.Error = errorDetail;
        }
    }
}
