using System;
using System.Net;
using System.Net.Http;

using Microsoft.Extensions.DependencyInjection;

using Polly;

using LightningPay.Clients.Lnd;

namespace LightningPay
{
    /// <summary>
    ///   LND dependency injection extension methods
    /// </summary>
    public static class LndClientExtensions
    {
        /// <summary>Adds the LND lightning client.</summary>
        /// <param name="services">The services.</param>
        /// <param name="address">The address of the LND server.</param>
        /// <returns>
        ///   ServiceCollection
        /// </returns>
        public static IServiceCollection AddLndLightningClient(this IServiceCollection services,
            Uri address)
        {
            return AddLndLightningClient(services,
                address, 
                macaroonHexString: null,
                allowInsecure: false,
                certificateThumbprint: null);
        }

        /// <summary>Adds the LND lightning client.</summary>
        /// <param name="services">The services.</param>
        /// <param name="address">The address of the LND server.</param>
        /// <param name="macaroonHexString">The macaroon hexadecimal string.</param>
        /// <returns>
        ///   ServiceCollection
        /// </returns>
        public static IServiceCollection AddLndLightningClient(this IServiceCollection services,
            Uri address,
            string macaroonHexString)
        {
            return AddLndLightningClient(services,
                address, 
                macaroonHexString.HexStringToByteArray(), 
                allowInsecure: false, 
                certificateThumbprint: null);
        }

        /// <summary>Adds the LND lightning client.</summary>
        /// <param name="services">The services.</param>
        /// <param name="address">The address of the LND server.</param>
        /// <param name="macaroonHexString">The macaroon hexadecimal string.</param>
        /// <param name="retryOnHttpError">Number of retry on http error</param>
        /// <param name="allowInsecure">if set to <c>true</c> [allow insecure].</param>
        /// <param name="certificateThumbprint">The certificate thumbprint.</param>
        /// <returns>
        ///   ServiceCollection
        /// </returns>
        public static IServiceCollection AddLndLightningClient(this IServiceCollection services,
           Uri address,
           string macaroonHexString = null,
           int retryOnHttpError = 10,
           bool allowInsecure = false,
           string certificateThumbprint = null)
        {
            return AddLndLightningClient(services,
                address,
                macaroonHexString.HexStringToByteArray(),
                retryOnHttpError: retryOnHttpError,
                allowInsecure: allowInsecure,
                certificateThumbprint: certificateThumbprint);
        }

        /// <summary>Adds the LND lightning client.</summary>
        /// <param name="services">The services.</param>
        /// <param name="address">The address of the LND server.</param>
        /// <param name="macaroonBytes">The macaroon bytes.</param>
        /// <param name="retryOnHttpError">Number of retry on http error</param>
        /// <param name="allowInsecure">if set to <c>true</c> [allow insecure].</param>
        /// <param name="certificateThumbprint">The certificate thumbprint.</param>
        /// <returns>
        ///   ServiceCollection
        /// </returns>
        public static IServiceCollection AddLndLightningClient(this IServiceCollection services,
            Uri address,
            byte[] macaroonBytes = null,
            int retryOnHttpError = 10,
            bool allowInsecure = false,
            string certificateThumbprint = null)
        {
            services.AddSingleton(new LndOptions()
            {
                Address = address,
                Macaroon = macaroonBytes
            });


            services.AddSingleton(new DependencyInjection.HttpClientHandlerOptions()
            {
                AllowInsecure = allowInsecure,
                CertificateThumbprint = certificateThumbprint.HexStringToByteArray()
            });
            services.AddSingleton<DependencyInjection.DefaultHttpClientHandler>();

            services.AddHttpClient<ILightningClient, LndClient>()
                 .AddPolicyHandler(Policy
                    // Add custom handling to exclude 500 because api return 500 for bad request errors
                    .Handle<HttpRequestException>()
                    .OrResult<HttpResponseMessage>(r =>  (int)r.StatusCode > 500 || r.StatusCode == HttpStatusCode.RequestTimeout)
                    .WaitAndRetryAsync(retryOnHttpError, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
                .ConfigurePrimaryHttpMessageHandler<DependencyInjection.DefaultHttpClientHandler>();

            return services;
        }
    }
}
