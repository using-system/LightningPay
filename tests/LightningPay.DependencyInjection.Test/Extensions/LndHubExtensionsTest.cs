using Microsoft.Extensions.DependencyInjection;

using Xunit;
using NSubstitute;

using LightningPay.Clients.LndHub;
using System.Net.Http;
using System.IO;

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
        }

        [Fact]
        public void AddLndHubLightningClient_Should_Add_LndHubClient_If_Allow_Insecure_Setted()
        {
            // Arrange
            var serviceCollection = Substitute.ForPartsOf<ServiceCollection>();

            // Act
            serviceCollection.AddLndHubLightningClient(new System.Uri("https://lndhub.herokuapp.com/"), 
                "login",
                "password",
                allowInsecure: true);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Assert
            var lightningClient = serviceProvider.GetService<ILightningClient>();
            Assert.NotNull(lightningClient);
            Assert.IsType<LndHubClient>(lightningClient);
        }

        [Fact]
        public void AddLndHubLightningClient_Should_AddLndHubClient_If_CertificateThumbprint_Setted()
        {
            // Arrange
            var serviceCollection = Substitute.ForPartsOf<ServiceCollection>();

            // Act
            serviceCollection.AddLndHubLightningClient(new System.Uri("https://lndhub.herokuapp.com/"),
                "login",
                "password",
                certificateThumbprint: File.ReadAllBytes((Path.Combine($"{Directory.GetCurrentDirectory()}/Seed/", "TestCertificate.cer"))));
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Assert
            var lightningClient = serviceProvider.GetService<ILightningClient>();
            Assert.NotNull(lightningClient);
            Assert.IsType<LndHubClient>(lightningClient);

        }
    }
}
