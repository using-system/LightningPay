using System;

using Microsoft.Extensions.DependencyInjection;

using Polly;
using Polly.Extensions.Http;

using LightningPay.Clients.LndHub;

namespace LightningPay
{
    /// <summary>
    ///   LNDHub dependency injection extension methods
    /// </summary>
    public static class LndHubExtensions
    {
        /// <summary>Adds the LND hub lightning client.</summary>
        /// <param name="services">The services.</param>
        /// <param name="address">The address of the LNDHub.</param>
        /// <param name="login">The login.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        ///   ServiceCollection
        /// </returns>
        public static IServiceCollection AddLndHubLightningClient(this IServiceCollection services,
            Uri address,
            string login,
            string password)
        {
            return AddLndHubLightningClient(services,
                address,
                login,
                password,
                allowInsecure: false,
                certificateThumbprint: null);
        }

        /// <summary>Adds the LND hub lightning client.</summary>
        /// <param name="services">The services.</param>
        /// <param name="address">The address of the LNDHub.</param>
        /// <param name="login">The login.</param>
        /// <param name="password">The password.</param>
        /// <param name="allowInsecure">if set to <c>true</c> [allow insecure].</param>
        /// <param name="certificateThumbprint">The certificate thumbprint.</param>
        /// <returns>
        ///   ServiceCollection
        /// </returns>
        public static IServiceCollection AddLndHubLightningClient(this IServiceCollection services,
            Uri address,
            string login,
            string password,
            bool allowInsecure = false,
            string certificateThumbprint = null)
        {
            services.AddSingleton(new LndHubOptions()
            {
                Address = address,
                Login = login,
                Password = password
            });
            
            services.AddSingleton(new DependencyInjection.HttpClientHandlerOptions()
            {
                AllowInsecure = allowInsecure,
                CertificateThumbprint = certificateThumbprint.HexStringToByteArray()
            });
            services.AddSingleton<DependencyInjection.DefaultHttpClientHandler>();

            services.AddHttpClient<ILightningClient, LndHubClient>()
                 .AddPolicyHandler(HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(15, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
                .ConfigurePrimaryHttpMessageHandler<DependencyInjection.DefaultHttpClientHandler>();

            return services;
        }
    }
}
