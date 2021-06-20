using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

using Microsoft.Extensions.DependencyInjection;

using LightningPay.Clients.Lnd;


namespace LightningPay
{
    public static class LndExtensions
    {
        public static IServiceCollection AddLndLightningClient(this IServiceCollection services,
            Uri server)
        {
            return AddLndLightningClient(services, 
                server, 
                macaroon: null,
                allowInsecure: false,
                certificateThumbprint: null);
        }

        public static IServiceCollection AddLndLightningClient(this IServiceCollection services,
            Uri server,
            byte[] macaroon)
        {
            return AddLndLightningClient(services, 
                server, 
                macaroon, 
                allowInsecure: false, 
                certificateThumbprint: null);
        }

        public static IServiceCollection AddLndLightningClient(this IServiceCollection services,
           Uri server,
           string macaroonFilePath)
        {
            return AddLndLightningClient(services, 
                server, 
                File.ReadAllBytes(macaroonFilePath), 
                allowInsecure: false, 
                certificateThumbprint: null);
        }

        public static IServiceCollection AddLndLightningClient(this IServiceCollection services,
            Uri server,
            string macaroonFilePath,
            bool allowInsecure = false,
            byte[] certificateThumbprint = null)
        {
            return AddLndLightningClient(services, 
                server, 
                File.ReadAllBytes(macaroonFilePath), 
                allowInsecure, 
                certificateThumbprint);
        }

        public static IServiceCollection AddLndLightningClient(this IServiceCollection services,
            Uri server,
            byte[] macaroon = null,
            bool allowInsecure = false,
            byte[] certificateThumbprint = null)
        {
            services.AddSingleton<ILightningClient, LndClient>();

            services.AddSingleton(new LndOptions()
            {
                BaseUri = server,
                Macaroon = macaroon
            });

            services.AddHttpClient<LndClient>().ConfigurePrimaryHttpMessageHandler(serviceProvider =>
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

            return services;
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
