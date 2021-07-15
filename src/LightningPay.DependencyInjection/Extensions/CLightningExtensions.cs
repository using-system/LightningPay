using System;

using Microsoft.Extensions.DependencyInjection;

using LightningPay.Clients.CLightning;

namespace LightningPay.DependencyInjection
{
    /// <summary>
    ///  C-Lightning client dependency injection extension methods
    /// </summary>
    public static class CLightningExtensions
    {
        /// <summary>Adds the C-Lightning client.</summary>
        /// <param name="services">The services.</param>
        /// <param name="address">The tcp address of the C-Lightning server.</param>
        /// <returns>
        ///   ServiceCollection
        /// </returns>
        public static IServiceCollection AddCLightningClient(this IServiceCollection services, Uri address)
        {
            services.AddSingleton(new CLightningOptions()
            {
                Address = address,
            });


            services.AddSingleton<IRpcClient, DefaultCLightningRpcClient>();
            services.AddSingleton<ILightningClient, CLightningClient>();

            return services;
        }
    }
}
