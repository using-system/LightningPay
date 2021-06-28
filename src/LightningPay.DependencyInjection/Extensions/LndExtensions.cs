using System;
using System.IO;

using Microsoft.Extensions.DependencyInjection;

using LightningPay.Clients.Lnd;


namespace LightningPay
{
    public static class LndExtensions
    {
        public static IServiceCollection AddLndLightningClient(this IServiceCollection services,
            Uri address)
        {
            return AddLndLightningClient(services,
                address, 
                macaroon: null,
                allowInsecure: false,
                certificateThumbprint: null);
        }

        public static IServiceCollection AddLndLightningClient(this IServiceCollection services,
            Uri address,
            byte[] macaroon)
        {
            return AddLndLightningClient(services,
                address, 
                macaroon, 
                allowInsecure: false, 
                certificateThumbprint: null);
        }

        public static IServiceCollection AddLndLightningClient(this IServiceCollection services,
           Uri address,
           string macaroonFilePath)
        {
            return AddLndLightningClient(services,
                address, 
                File.ReadAllBytes(macaroonFilePath), 
                allowInsecure: false, 
                certificateThumbprint: null);
        }

        public static IServiceCollection AddLndLightningClient(this IServiceCollection services,
            Uri address,
            string macaroonFilePath,
            bool allowInsecure = false,
            byte[] certificateThumbprint = null)
        {
            return AddLndLightningClient(services,
                address, 
                File.ReadAllBytes(macaroonFilePath), 
                allowInsecure, 
                certificateThumbprint);
        }

        public static IServiceCollection AddLndLightningClient(this IServiceCollection services,
            Uri address,
            byte[] macaroon = null,
            bool allowInsecure = false,
            byte[] certificateThumbprint = null)
        {
            services.AddSingleton(new LndOptions()
            {
                Address = address,
                Macaroon = macaroon
            });
            services.AddSingleton<ILightningClient, LndClient>();


            services.AddSingleton(new DependencyInjection.HttpClientHandlerOptions()
            {
                AllowInsecure = allowInsecure,
                CertificateThumbprint = certificateThumbprint
            });
            services.AddSingleton<DependencyInjection.DefaultHttpClientHandler>();
            services.AddHttpClient<ILightningClient, LndClient>()
                .ConfigurePrimaryHttpMessageHandler<DependencyInjection.DefaultHttpClientHandler>();

            return services;
        }
    }
}
