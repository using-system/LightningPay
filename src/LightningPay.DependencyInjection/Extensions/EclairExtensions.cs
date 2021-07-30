using System;

using Microsoft.Extensions.DependencyInjection;

using Polly;
using Polly.Extensions.Http;

using LightningPay.Clients.Eclair;

namespace LightningPay
{
    /// <summary>
    ///  Eclair dependency injection extension methods
    /// </summary>
    public static class EclairExtensions
    {
        /// <summary>Adds the LNBits lightning client.</summary>
        /// <param name="services">The services.</param>
        /// <param name="address">The address of the LNBits api.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        ///   ServiceCollection
        /// </returns>
        public static IServiceCollection AddEclairLightningClient(this IServiceCollection services,
            Uri address,
            string password)
        {
            return AddEclairLightningClient(services,
                address,
                password,
                allowInsecure: false,
                certificateThumbprint: null);
        }


        /// <summary>Adds the Eclair lightning client.</summary>
        /// <param name="services">The services.</param>
        /// <param name="address">The address of the LNBits api.</param>
        /// <param name="password">The password.</param>
        /// <param name="retryOnHttpError">Number of retry on http error</param>
        /// <param name="allowInsecure">if set to <c>true</c> [allow insecure].</param>
        /// <param name="certificateThumbprint">The certificate thumbprint.</param>
        /// <returns>ServiceCollection</returns>
        public static IServiceCollection AddEclairLightningClient(this IServiceCollection services,
            Uri address,
            string password,
            int retryOnHttpError = 10,
            bool allowInsecure = false,
            string certificateThumbprint = null)
        {
            services.AddSingleton(new EclairOptions()
            {
                Address = address,
                Password = password
            });

            services.AddSingleton(new DependencyInjection.HttpClientHandlerOptions()
            {
                AllowInsecure = allowInsecure,
                CertificateThumbprint = certificateThumbprint.HexStringToByteArray()
            });
            services.AddSingleton<DependencyInjection.DefaultHttpClientHandler>();

            services.AddHttpClient<ILightningClient, EclairClient>()
                .AddPolicyHandler(HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(retryOnHttpError, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
                .ConfigurePrimaryHttpMessageHandler<DependencyInjection.DefaultHttpClientHandler>();

            return services;
        }
    }
}
