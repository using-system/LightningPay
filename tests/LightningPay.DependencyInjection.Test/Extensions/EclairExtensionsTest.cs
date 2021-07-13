using System;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using Xunit;
using NSubstitute;

using LightningPay.Clients.Eclair;

namespace LightningPay.DependencyInjection.Test.Extensions
{
    public class EclairExtensionsTest
    {
        [Fact]
        public void AddEclairLightningClient_Should_Add_EclairClient()
        {
            // Arrange
            var serviceCollection = Substitute.ForPartsOf<ServiceCollection>();

            // Act
            serviceCollection.AddEclairLightningClient(new Uri("http://localhost:8080"), "password");
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Assert
            var options = serviceProvider.GetService<EclairOptions>();
            Assert.NotNull(options);
            Assert.Equal("http://localhost:8080/", options.Address.ToString());
            Assert.Equal("password", options.Password);

            var lightningClient = serviceProvider.GetService<ILightningClient>();
            Assert.NotNull(lightningClient);
            Assert.IsType<EclairClient>(lightningClient);

            var httpHandler = serviceProvider.GetService<DependencyInjection.DefaultHttpClientHandler>();
            Assert.NotNull(httpHandler);

            var httpHandlerOptions = serviceProvider.GetService<HttpClientHandlerOptions>();
            Assert.NotNull(httpHandlerOptions);
            Assert.False(httpHandlerOptions.AllowInsecure);
            Assert.Null(httpHandlerOptions.CertificateThumbprint);
        }

        [Fact]
        public void AddEclairLightningClient_Should_Add_EclairClient_With_Certificate_Options()
        {
            // Arrange
            var serviceCollection = Substitute.ForPartsOf<ServiceCollection>();

            // Act
            serviceCollection.AddEclairLightningClient(new Uri("http://localhost:8080"),
                "password",
                certificateThumbprint: "284800A04D0C046636EBE60C37A4F527B8B550F3");
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Assert
            var options = serviceProvider.GetService<EclairOptions>();
            Assert.NotNull(options);
            Assert.Equal("http://localhost:8080/", options.Address.ToString());
            Assert.Equal("password", options.Password);

            var lightningClient = serviceProvider.GetService<ILightningClient>();
            Assert.NotNull(lightningClient);
            Assert.IsType<EclairClient>(lightningClient);

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
