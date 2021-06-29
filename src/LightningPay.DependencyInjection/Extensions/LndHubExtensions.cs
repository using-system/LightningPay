using System;

using Microsoft.Extensions.DependencyInjection;

using LightningPay.Clients.LndHub;

namespace LightningPay
{
    public static class LndHubExtensions
    {
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
            services.AddSingleton<ILightningClient, LndHubClient>();
            
            services.AddSingleton(new DependencyInjection.HttpClientHandlerOptions()
            {
                AllowInsecure = allowInsecure,
                CertificateThumbprint = certificateThumbprint.HexStringToByteArray()
            });
            services.AddSingleton<DependencyInjection.DefaultHttpClientHandler>();
            services.AddHttpClient<ILightningClient, LndHubClient>()
                .ConfigurePrimaryHttpMessageHandler<DependencyInjection.DefaultHttpClientHandler>();

            return services;
        }
    }
}
