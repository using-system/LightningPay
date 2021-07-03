using System.Net.Http;

using Xunit;

using LightningPay.Clients.LNBits;

namespace LightningPay.Test.Clients.LNBits
{
    public class LNBitsAuthenticationTest
    {
        [Fact]
        public void AddAuthentication_Should_Not_Add_ApiKey_Header_If_ApiKey_Is_Null()
        {
            //Arrange
            HttpRequestMessage request = new HttpRequestMessage();
            string apiKey = null;
            var lnBitsAuthentication = new LNBitsAuthentication(apiKey);

            //Act
            lnBitsAuthentication.AddAuthentication(null, request);

            //Assert
            Assert.False(request.Headers.Contains(LNBitsAuthentication.HEADER_KEY));
        }

        [Fact]
        public void AddAuthentication_Should_Add_ApiKey_Header()
        {
            //Arrange
            HttpRequestMessage request = new HttpRequestMessage();
            string apiKey = "ApiKey";
            var lnBitsAuthentication = new LNBitsAuthentication(apiKey);

            //Act
            lnBitsAuthentication.AddAuthentication(null, request);

            //Assert
            Assert.True(request.Headers.Contains(LNBitsAuthentication.HEADER_KEY));
            var headerValues = request.Headers.GetValues(LNBitsAuthentication.HEADER_KEY);
            Assert.Single(headerValues, "ApiKey");
        }
    }
}
