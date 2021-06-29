using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using Xunit;
using NSubstitute;

using LightningPay.Clients.LndHub;

namespace LightningPay.DependencyInjection.Test
{
    public class LndHubExtensionsTest
    {
        [Fact]
        public void AddLndHubLightningClient_Should_Add_LndHubClient()
        {
            // Arrange
            var serviceCollection = Substitute.ForPartsOf<ServiceCollection>();

            // Act
            serviceCollection.AddLndHubLightningClient(new System.Uri("https://lndhub.herokuapp.com/"), "login", "password");
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Assert
            var options = serviceProvider.GetService<LndHubOptions>();
            Assert.NotNull(options);
            Assert.Equal("https://lndhub.herokuapp.com/", options.Address.ToString());
            Assert.Equal("login", options.Login);
            Assert.Equal("password", options.Password);

            var lightningClient = serviceProvider.GetService<ILightningClient>();
            Assert.NotNull(lightningClient);
            Assert.IsType<LndHubClient>(lightningClient);

            var httpHandler = serviceProvider.GetService<DependencyInjection.DefaultHttpClientHandler>();
            Assert.NotNull(httpHandler);

            var httpHandlerOptions = serviceProvider.GetService<HttpClientHandlerOptions>();
            Assert.NotNull(httpHandlerOptions);
            Assert.False(httpHandlerOptions.AllowInsecure);
            Assert.Null(httpHandlerOptions.CertificateThumbprint);
        }

        [Fact]
        public void AddLndHubLightningClient_Should_Add_LndHubClient_With_Certificate_Options()
        {
            // Arrange
            var serviceCollection = Substitute.ForPartsOf<ServiceCollection>();

            // Act
            serviceCollection.AddLndHubLightningClient(new System.Uri("https://lndhub.herokuapp.com/"), 
                "login", 
                "password",
                certificateThumbprint: "284800A04D0C046636EBE60C37A4F527B8B550F3");
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Assert
            var options = serviceProvider.GetService<LndHubOptions>();
            Assert.NotNull(options);
            Assert.Equal("https://lndhub.herokuapp.com/", options.Address.ToString());
            Assert.Equal("login", options.Login);
            Assert.Equal("password", options.Password);

            var lightningClient = serviceProvider.GetService<ILightningClient>();
            Assert.NotNull(lightningClient);
            Assert.IsType<LndHubClient>(lightningClient);

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
