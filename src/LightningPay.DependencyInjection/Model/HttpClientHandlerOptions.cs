namespace LightningPay.DependencyInjection
{
    /// <summary>
    ///   Http Client Handler Options
    /// </summary>
    public class HttpClientHandlerOptions
    {
        /// <summary>Gets or sets a value indicating whether [allow insecure].</summary>
        /// <value>
        ///   <c>true</c> if [allow insecure]; otherwise, <c>false</c>.</value>
        public bool AllowInsecure { get; set; }

        /// <summary>Gets or sets the certificate thumbprint.</summary>
        /// <value>The certificate thumbprint.</value>
        public byte[] CertificateThumbprint { get; set; }
    }
}
