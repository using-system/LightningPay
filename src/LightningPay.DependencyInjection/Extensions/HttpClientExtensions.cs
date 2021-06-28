using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

using Microsoft.Extensions.DependencyInjection;

namespace LightningPay
{
    internal static class HttpClientExtensions
    {
        internal static IHttpClientBuilder ConfigureHttpHandler(this IHttpClientBuilder builder,
            bool allowInsecure = false,
            byte[] certificateThumbprint = null) 
        {            
            builder.ConfigurePrimaryHttpMessageHandler(serviceProvider =>
            {
                var handler = new HttpClientHandler();

                if (allowInsecure)
                {
                    handler.ServerCertificateCustomValidationCallback = (request, cert, chain, errors) => true;
                }
                else if (certificateThumbprint != null)
                {
                    var expectedThumbprint = certificateThumbprint.ToArray();

                    handler.ServerCertificateCustomValidationCallback = (request, cert, chain, errors) =>
                    {
                        var actualCert = chain.ChainElements[chain.ChainElements.Count - 1].Certificate;
                        var hash = GetHash(actualCert);
                        return hash.SequenceEqual(expectedThumbprint);
                    };

                }

                return handler;
            });

            return builder;
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
