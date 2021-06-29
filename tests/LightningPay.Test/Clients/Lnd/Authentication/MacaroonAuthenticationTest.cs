using System.Net.Http;

using Xunit;

using LightningPay.Clients.Lnd;


namespace LightningPay.Test.Clients.Lnd
{
    public class MacaroonAuthenticationTest
    {
        [Fact]
        public void AddAuthentication_Should_Not_Add_Grpc_Header_If_Macaroon_Is_Null()
        {
            //Arrange
            HttpRequestMessage request = new HttpRequestMessage();
            byte[] macaroon = null;
            var macaroonAuth = new MacaroonAuthentication(macaroon);

            //Act
            macaroonAuth.AddAuthentication(null, request);

            //Assert
            Assert.False(request.Headers.Contains("Grpc-Metadata-macaroon"));
        }

        [Fact]
        public void AddAuthentication_Should_Add_Grpc_Header()
        {
            //Arrange
            HttpRequestMessage request = new HttpRequestMessage();
            var macaroon = new byte[] { 1, 2, 4, 8, 16, 32 };
            var macaroonAuth = new MacaroonAuthentication(macaroon);

            //Act
            macaroonAuth.AddAuthentication(null, request);

            //Assert
            Assert.True(request.Headers.Contains("Grpc-Metadata-macaroon"));
            var headerValues = request.Headers.GetValues("Grpc-Metadata-macaroon");
            Assert.Single(headerValues, macaroon.ToBitString());
        }
    }
}
