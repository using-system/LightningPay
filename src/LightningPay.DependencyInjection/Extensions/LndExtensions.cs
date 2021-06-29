using System;

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
                macaroonHexString: null,
                allowInsecure: false,
                certificateThumbprint: null);
        }

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

        public static IServiceCollection AddLndLightningClient(this IServiceCollection services,
           Uri address,
           string macaroonHexString = null,
           bool allowInsecure = false,
           string certificateThumbprint = null)
        {
            return AddLndLightningClient(services,
                address,
                macaroonHexString.HexStringToByteArray(),
                allowInsecure,
                certificateThumbprint);
        }

        public static IServiceCollection AddLndLightningClient(this IServiceCollection services,
            Uri address,
            byte[] macaroonBytes = null,
            bool allowInsecure = false,
            string certificateThumbprint = null)
        {
            services.AddSingleton(new LndOptions()
            {
                Address = address,
                Macaroon = macaroonBytes
            });
            services.AddSingleton<ILightningClient, LndClient>();


            services.AddSingleton(new DependencyInjection.HttpClientHandlerOptions()
            {
                AllowInsecure = allowInsecure,
                CertificateThumbprint = certificateThumbprint.HexStringToByteArray()
            });
            services.AddSingleton<DependencyInjection.DefaultHttpClientHandler>();
            services.AddHttpClient<ILightningClient, LndClient>()
                .ConfigurePrimaryHttpMessageHandler<DependencyInjection.DefaultHttpClientHandler>();

            return services;
        }
    }
}
