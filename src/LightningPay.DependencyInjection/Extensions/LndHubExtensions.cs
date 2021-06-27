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
            byte[] certificateThumbprint = null)
        {
            services.AddSingleton<ILightningClient, LndHubClient>();

            services.AddSingleton(new LndHubOptions()
            {
                Address = address,
                Login = login,
                Password = password
            });

            services.AddHttpClient<LndHubClient>()
                .ConfigureHttpHandler<LndHubClient>(allowInsecure, certificateThumbprint);

            return services;
        }
    }
}
