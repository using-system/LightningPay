using System;

using Microsoft.Extensions.DependencyInjection;

using Xunit;
using NSubstitute;

using LightningPay.Clients.CLightning;

namespace LightningPay.DependencyInjection.Test.Extensions
{
    public class CLightningExtensionsTest
    {
        [Fact]
        public void AddCLightningClient_Should_Add_CLightningClient()
        {
            // Arrange
            var serviceCollection = Substitute.ForPartsOf<ServiceCollection>();

            // Act
            serviceCollection.AddCLightningClient(new Uri("tcp://127.0.0.1:48532/"));
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Assert
            var options = serviceProvider.GetService<CLightningOptions>();
            Assert.NotNull(options);
            Assert.Equal("tcp://127.0.0.1:48532/", options.Address.ToString());

            var tcpClient = serviceProvider.GetService<ICLightningTcpClient>();
            Assert.NotNull(tcpClient);

            var lightningClient = serviceProvider.GetService<ILightningClient>();
            Assert.NotNull(lightningClient);
            Assert.IsType<CLightningClient>(lightningClient);
        }
    }
}
