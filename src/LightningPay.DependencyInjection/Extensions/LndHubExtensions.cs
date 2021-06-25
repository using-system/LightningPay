using System;

using Microsoft.Extensions.DependencyInjection;

using LightningPay.Clients.LndHub;

namespace LightningPay
{
    public static class LndHubExtensions
    {
        public static IServiceCollection AddLndHubLightningClient(this IServiceCollection services,
            Uri server,
            string login,
            string password)
        {
            return AddLndHubLightningClient(services,
                server,
                login,
                password,
                allowInsecure: false,
                certificateThumbprint: null);
        }

        public static IServiceCollection AddLndHubLightningClient(this IServiceCollection services,
            Uri server,
            string login,
            string password,
            bool allowInsecure = false,
            byte[] certificateThumbprint = null)
        {
            services.AddSingleton<ILightningClient, LndHubClient>();

            services.AddSingleton(new LndHubOptions()
            {
                BaseUri = server,
                Login = login,
                Password = password
            });

            services.AddHttpClient<LndHubClient>()
                .ConfigureHttpHandler<LndHubClient>(allowInsecure, certificateThumbprint);

            return services;
        }
    }
}
