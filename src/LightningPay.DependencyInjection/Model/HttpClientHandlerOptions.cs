namespace LightningPay.DependencyInjection
{
    public class HttpClientHandlerOptions
    {
        public bool AllowInsecure { get; set; }

        public byte[] CertificateThumbprint { get; set; }
    }
}
