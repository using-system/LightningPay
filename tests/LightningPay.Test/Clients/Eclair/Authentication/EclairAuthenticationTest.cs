using System.Net.Http;

using Xunit;

using LightningPay.Clients.Eclair;

namespace LightningPay.Test.Clients.Eclair
{
    public class EclairAuthenticationTest
    {
        [Fact]
        public void AddAuthentication_Should_Not_Add_Header_Authorization_If_Password_Is_Null()
        {
            //Arrange
            HttpRequestMessage request = new HttpRequestMessage();
            string password = null;
            var lnBitsAuthentication = new EclairAuthentication(password);

            //Act
            lnBitsAuthentication.AddAuthentication(null, request);

            //Assert
            Assert.Null(request.Headers.Authorization);
        }

        [Fact]
        public void AddAuthentication_Should_Add_Header_Authorization()
        {
            //Arrange
            HttpRequestMessage request = new HttpRequestMessage();
            string password = "password";
            var lnBitsAuthentication = new EclairAuthentication(password);

            //Act
            lnBitsAuthentication.AddAuthentication(null, request);

            //Assert
            Assert.NotNull(request.Headers.Authorization);
        }
    }
}
