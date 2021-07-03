using System;

using Microsoft.Extensions.DependencyInjection;

using Xunit;
using NSubstitute;

using LightningPay.Clients.Lnd;

namespace LightningPay.DependencyInjection.Test
{
    public class LndHExtensionsTest
    {

        [Fact]
        public void AddLndLightningClient_Should_Add_LndClient()
        {
            // Arrange
            var serviceCollection = Substitute.ForPartsOf<ServiceCollection>();

            // Act
            serviceCollection.AddLndLightningClient(new Uri("http://localhost:42802/"), macaroonHexString: "971a5512");
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Assert
            var options = serviceProvider.GetService<LndOptions>();
            Assert.NotNull(options);
            Assert.Equal("http://localhost:42802/", options.Address.ToString());
            Assert.Equal(new byte[] { 151, 26, 85, 18 }, options.Macaroon);

            var lightningClient = serviceProvider.GetService<ILightningClient>();
            Assert.NotNull(lightningClient);
            Assert.IsType<LndClient>(lightningClient);

            var httpHandler = serviceProvider.GetService<DefaultHttpClientHandler>();
            Assert.NotNull(httpHandler);
        }

        [Fact]
        public void AddLndLightningClient_Without_Macaroon_Should_Add_LndClient()
        {
            // Arrange
            var serviceCollection = Substitute.ForPartsOf<ServiceCollection>();

            // Act
            serviceCollection.AddLndLightningClient(new Uri("http://localhost:42802/"));
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Assert
            var options = serviceProvider.GetService<LndOptions>();
            Assert.NotNull(options);
            Assert.Equal("http://localhost:42802/", options.Address.ToString());
            Assert.Null(options.Macaroon);

            var lightningClient = serviceProvider.GetService<ILightningClient>();
            Assert.NotNull(lightningClient);
            Assert.IsType<LndClient>(lightningClient);

            var httpHandler = serviceProvider.GetService<DefaultHttpClientHandler>();
            Assert.NotNull(httpHandler);

            var httpHandlerOptions = serviceProvider.GetService<HttpClientHandlerOptions>();
            Assert.NotNull(httpHandlerOptions);
            Assert.False(httpHandlerOptions.AllowInsecure);
            Assert.Null(httpHandlerOptions.CertificateThumbprint);
        }
    }
}
