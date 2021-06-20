using System;

using Microsoft.Extensions.DependencyInjection;

using LightningPay.Clients.Lnd;

namespace LightningPay
{ 
    public static class LndExtensions
    {
        public static IServiceCollection AddLndLightningClient(this IServiceCollection services,
            Uri server)
        {
            services.AddSingleton<ILightningClient, LndClient>();
            services.AddSingleton(new LndOptions()
            {
                BaseUri = server
            });
            services.AddHttpClient<LndClient>();

            return services;
        }
    }
}
