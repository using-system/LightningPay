using System;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using Xunit;
using NSubstitute;

using LightningPay.Clients.LNBits;

namespace LightningPay.DependencyInjection.Test
{
    public class LNBitsExtensionsTest
    {
        [Fact]
        public void AddLNBitsLightningClient_Should_Add_LNBitsClient()
        {
            // Arrange
            var serviceCollection = Substitute.ForPartsOf<ServiceCollection>();

            // Act
            serviceCollection.AddLNBitsLightningClient(new Uri("https://lnbits.com/"), "apikey");
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Assert
            var options = serviceProvider.GetService<LNBitsOptions>();
            Assert.NotNull(options);
            Assert.Equal("https://lnbits.com/", options.Address.ToString());
            Assert.Equal("apikey", options.ApiKey);

            var lightningClient = serviceProvider.GetService<ILightningClient>();
            Assert.NotNull(lightningClient);
            Assert.IsType<LNBitsClient>(lightningClient);

            var httpHandler = serviceProvider.GetService<DependencyInjection.DefaultHttpClientHandler>();
            Assert.NotNull(httpHandler);

            var httpHandlerOptions = serviceProvider.GetService<HttpClientHandlerOptions>();
            Assert.NotNull(httpHandlerOptions);
            Assert.False(httpHandlerOptions.AllowInsecure);
            Assert.Null(httpHandlerOptions.CertificateThumbprint);
        }

        [Fact]
        public void AddLNBitsLightningClient_Should_Add_LNBitsClient_With_Certificate_Options()
        {
            // Arrange
            var serviceCollection = Substitute.ForPartsOf<ServiceCollection>();

            // Act
            serviceCollection.AddLNBitsLightningClient(new Uri("https://lnbits.com/"),
                "apikey",
                certificateThumbprint: "284800A04D0C046636EBE60C37A4F527B8B550F3");
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Assert
            var options = serviceProvider.GetService<LNBitsOptions>();
            Assert.NotNull(options);
            Assert.Equal("https://lnbits.com/", options.Address.ToString());
            Assert.Equal("apikey", options.ApiKey);

            var lightningClient = serviceProvider.GetService<ILightningClient>();
            Assert.NotNull(lightningClient);
            Assert.IsType<LNBitsClient>(lightningClient);

            var httpHandler = serviceProvider.GetService<DependencyInjection.DefaultHttpClientHandler>();
            Assert.NotNull(httpHandler);

            var httpHandlerOptions = serviceProvider.GetService<HttpClientHandlerOptions>();
            Assert.NotNull(httpHandlerOptions);
            Assert.False(httpHandlerOptions.AllowInsecure);
            Assert.NotNull(httpHandlerOptions.CertificateThumbprint);
            Assert.Equal(40, httpHandlerOptions.CertificateThumbprint.First());
        }
    }
}
