using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace LightningPay.DependencyInjection
{
    public class DefaultHttpClientHandler : HttpClientHandler
    {
        public DefaultHttpClientHandler(HttpClientHandlerOptions options)
        {
            if (options.AllowInsecure)
            {
                this.ServerCertificateCustomValidationCallback = (request, cert, chain, errors) => true;
            }
            else if (options.CertificateThumbprint != null)
            {
                var expectedThumbprint = options.CertificateThumbprint.ToArray();

                this.ServerCertificateCustomValidationCallback = (request, cert, chain, errors) =>
                {
                    var actualCert = chain.ChainElements[chain.ChainElements.Count - 1].Certificate;
                    return actualCert.Thumbprint.HexStringToByteArray().SequenceEqual(expectedThumbprint);
                };

            }
        }

        private static byte[] GetHash(X509Certificate2 cert)
        {
            using (HashAlgorithm alg = SHA256.Create())
            {
                return alg.ComputeHash(cert.RawData);
            }
        }
    }
}
