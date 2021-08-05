using System;

using Microsoft.Extensions.DependencyInjection;

using Polly;
using Polly.Extensions.Http;

using LightningPay.Clients.LNBits;

namespace LightningPay
{
    /// <summary>
    /// LNBits dependency injection extension methods
    /// </summary>
    public static class LNBitsClientExtensions
    {
        /// <summary>Adds the LNBits lightning client.</summary>
        /// <param name="services">The services.</param>
        /// <param name="address">The address of the LNBits api.</param>
        /// <param name="apiKey">The API key.</param>
        /// <returns>
        ///   ServiceCollection
        /// </returns>
        public static IServiceCollection AddLNBitsLightningClient(this IServiceCollection services,
            Uri address,
            string apiKey)
        {
            return AddLNBitsLightningClient(services,
                address,
                apiKey,
                allowInsecure: false,
                certificateThumbprint: null);
        }


        /// <summary>Adds the LNBits lightning client.</summary>
        /// <param name="services">The services.</param>
        /// <param name="address">The address of the LNBits api.</param>
        /// <param name="apiKey">The api key.</param>
        /// <param name="retryOnHttpError">Number of retry on http error</param>
        /// <param name="allowInsecure">if set to <c>true</c> [allow insecure].</param>
        /// <param name="certificateThumbprint">The certificate thumbprint.</param>
        /// <returns>
        ///   ServiceCollection
        /// </returns>
        public static IServiceCollection AddLNBitsLightningClient(this IServiceCollection services,
            Uri address,
            string apiKey,
            int retryOnHttpError = 10,
            bool allowInsecure = false,
            string certificateThumbprint = null)
        {
            services.AddSingleton(new LNBitsOptions()
            {
                Address = address,
                ApiKey = apiKey
            });

            services.AddSingleton(new DependencyInjection.HttpClientHandlerOptions()
            {
                AllowInsecure = allowInsecure,
                CertificateThumbprint = certificateThumbprint.HexStringToByteArray()
            });
            services.AddSingleton<DependencyInjection.DefaultHttpClientHandler>();

            services.AddHttpClient<ILightningClient, LNBitsClient>()
                 .AddPolicyHandler(HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(retryOnHttpError, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
                .ConfigurePrimaryHttpMessageHandler<DependencyInjection.DefaultHttpClientHandler>();

            return services;
        }
    }
}
