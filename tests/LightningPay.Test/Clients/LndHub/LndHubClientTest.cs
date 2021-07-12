using System.Threading.Tasks;
using System.Net;
using System;
using System.Net.Http;

using Xunit;

using LightningPay.Clients.LndHub;
using LightningPay.Tools;
using LightningPay.Infrastructure.Api;

namespace LightningPay.Test.Clients.LndHub
{
    public class LndHubClientTest
    {
        [Fact]
        public async Task CreateInvoice_Should_Return_Lightning_Invoice()
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                (
                    Json.Serialize(new GetTokenResponse()
                    {
                        AccessToken = "AccessToken",
                        RefreshToken = "RefreshToken"
                    }), HttpStatusCode.OK
                ),
                (
                    Json.Serialize(new AddInvoiceResponse()
                    {
                        PaymentRequest = "PaymentRequest",
                        R_hash = new AddInvoiceResponse.Hash()
                        {
                            Data = new byte[] { 0, 1 }
                        }
                    }), HttpStatusCode.OK
                ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var lndClient = LndHubClient.New("https://lndhub.herokuapp.com/", "login", "password", httpClient);

            //Act
            var invoice = await lndClient.CreateInvoice(1000, "Test");

            //Assert
            Assert.Equal(2, mockMessageHandler.Requests.Count);
            Assert.True(mockMessageHandler.Requests[1].Headers.Contains("Authorization"));
            Assert.Single(mockMessageHandler.Requests[1].Headers.GetValues("Authorization"), "Bearer AccessToken");
            Assert.Equal("https://lndhub.herokuapp.com/auth?type=auth", mockMessageHandler.Requests[0].RequestUri.ToString());
            Assert.Equal("https://lndhub.herokuapp.com/addinvoice", mockMessageHandler.Requests[1].RequestUri.ToString());
            Assert.Equal(1000, invoice.Amount);
            Assert.Equal("Test", invoice.Memo);
            Assert.Equal(LightningInvoiceStatus.Unpaid, invoice.Status);
            Assert.Equal("0001", invoice.Id);
            Assert.Equal("PaymentRequest", invoice.BOLT11);
            Assert.True(invoice.ExpiresAt > DateTimeOffset.UtcNow.AddHours(23));
        }

        [Fact]
        public async Task CreateInvoice_Should_Throw_ApiException_If_Response_StatusCode_KO()
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                (
                   "error", HttpStatusCode.BadRequest
                ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var lndClient = LndHubClient.New("https://lndhub.herokuapp.com/", "login", "password", httpClient);

            //Act & Assert
            await Assert.ThrowsAsync<LightningPayException>(() => lndClient.CreateInvoice(1000, "Test"));
            Assert.Single(mockMessageHandler.Requests);
        }

        [Fact]
        public async Task CreateInvoice_Should_Throw_ApiException_If_Response_Failed()
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                (
                    Json.Serialize(new GetTokenResponse()
                    {
                        AccessToken = "AccessToken",
                        RefreshToken = "RefreshToken"
                    }), HttpStatusCode.OK
                ),
                (
                    Json.Serialize(new AddInvoiceResponse()
                    {
                        Failed = true
                    }), HttpStatusCode.OK
                ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var lndClient = LndHubClient.New("https://lndhub.herokuapp.com/", "login", "password", httpClient);

            //Act & Assert
            await Assert.ThrowsAsync<LightningPayException>(() => lndClient.CreateInvoice(1000, "Test"));
            Assert.Equal(2, mockMessageHandler.Requests.Count);
        }

        [Fact]
        public async Task CreateInvoice_Should_Throw_ApiException_If_Response_Not_Contains_Payment_Request()
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                 (
                    Json.Serialize(new GetTokenResponse()
                    {
                        AccessToken = "AccessToken",
                        RefreshToken = "RefreshToken"
                    }), HttpStatusCode.OK
                ),
                (
                    Json.Serialize(new AddInvoiceResponse()), HttpStatusCode.OK
                ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var lndClient = LndHubClient.New("https://lndhub.herokuapp.com/", "login", "password", httpClient);

            //Act & Assert
            await Assert.ThrowsAsync<LightningPayException>(() => lndClient.CreateInvoice(1000, "Test"));
            Assert.Equal(2, mockMessageHandler.Requests.Count);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task CheckPayment_Should_Return_Paid_Property(bool paidResponse)
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                 (
                     Json.Serialize(new GetTokenResponse()
                     {
                         AccessToken = "AccessToken",
                         RefreshToken = "RefreshToken"
                     }), HttpStatusCode.OK
                 ),
                 (
                     Json.Serialize(new CheckPaymentResponse()
                     {
                         Paid = paidResponse
                     }), HttpStatusCode.OK
                 ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var lndClient = LndHubClient.New("https://lndhub.herokuapp.com/", "login", "password", httpClient);

            //Act
            var actual = await lndClient.CheckPayment("id");

            //Assert
            Assert.True(actual == paidResponse);
            Assert.Equal(2, mockMessageHandler.Requests.Count);
            Assert.Equal("https://lndhub.herokuapp.com/auth?type=auth", mockMessageHandler.Requests[0].RequestUri.ToString());
            Assert.Equal("https://lndhub.herokuapp.com/checkpayment/id", mockMessageHandler.Requests[1].RequestUri.ToString());
        }

        [Fact]
        public async Task CheckPayment_Should_Throw_ApiException_If_Response_StatusCode_KO()
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                (
                   Json.Serialize(new GetTokenResponse()
                   {
                       AccessToken = "AccessToken",
                       RefreshToken = "RefreshToken"
                   }), HttpStatusCode.OK
               ),
               (
                   "error", HttpStatusCode.BadRequest
               ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var lndClient = LndHubClient.New("https://lndhub.herokuapp.com/", "login", "password", httpClient);

            //Act & Assert
            await Assert.ThrowsAsync<LightningPayException>(() => lndClient.CheckPayment("id"));
            Assert.Equal(2, mockMessageHandler.Requests.Count);
        }

        [Fact]
        public async Task GetBalance_Should_Return_Wallet_Balance()
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                (
                     Json.Serialize(new GetTokenResponse()
                     {
                         AccessToken = "AccessToken",
                         RefreshToken = "RefreshToken"
                     }), HttpStatusCode.OK
                 ),
                (
                    Json.Serialize(new GetBalanceResponse()
                    {
                        BTC = new GetBalanceResponse.BTCBalance()
                        {
                            AvailableBalance = 1500
                        }
                    }), HttpStatusCode.OK
                ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var lndClient = LndHubClient.New("https://lndhub.herokuapp.com/", "login", "password", httpClient: httpClient);

            //Act
            var actual = await lndClient.GetBalance();

            //Assert
            Assert.Equal(1500, actual);
            Assert.Equal(2, mockMessageHandler.Requests.Count);
            Assert.Equal("https://lndhub.herokuapp.com/balance", mockMessageHandler.Requests[1].RequestUri.ToString());
        }

        [Fact]
        public async Task Pay_Should_Return_Payment_Result()
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                (
                     Json.Serialize(new GetTokenResponse()
                     {
                         AccessToken = "AccessToken",
                         RefreshToken = "RefreshToken"
                     }), HttpStatusCode.OK
                ),
                (
                    Json.Serialize(new PayResponse()
                    {
                        Error = string.Empty
                    }), HttpStatusCode.OK
                ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var lndClient = LndHubClient.New("https://lndhub.herokuapp.com/", "login", "password", httpClient: httpClient);

            //Act
            var actual = await lndClient.Pay("request");

            //Assert
            Assert.True(actual);
            Assert.Equal(2, mockMessageHandler.Requests.Count);
            Assert.Equal("https://lndhub.herokuapp.com/payinvoice", mockMessageHandler.Requests[1].RequestUri.ToString());
        }

        [Fact]
        public async Task Pay_Should_Return_False_If_An_Payment_Error_Occurs()
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                (
                     Json.Serialize(new GetTokenResponse()
                     {
                         AccessToken = "AccessToken",
                         RefreshToken = "RefreshToken"
                     }), HttpStatusCode.OK
                ),
                (
                    Json.Serialize(new PayResponse()
                    {
                        Error = "no routes"
                    }), HttpStatusCode.OK
                ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var lndClient = LndHubClient.New("https://lndhub.herokuapp.com/", "login", "password", httpClient: httpClient);

            //Act
            var actual = await lndClient.Pay("request");

            //Assert
            Assert.False(actual);
            Assert.Equal(2, mockMessageHandler.Requests.Count);
            Assert.Equal("https://lndhub.herokuapp.com/payinvoice", mockMessageHandler.Requests[1].RequestUri.ToString());
        }

        [Fact]
        public void BuildAuthentication_Should_Return_LndHubAuthentication_If_Options_Contains_Login_And_Password()
        {
            //Arrange
            var options = new LndHubOptions()
            {
                Login = "login",
                Password = "password"
            };

            //Act
            var authentication = LndHubClient.BuildAuthentication(options);

            //Assert
            Assert.IsType<LndHubAuthentication>(authentication);
        }

        [Fact]
        public void BuildAuthentication_Should_Return_NoAuthentication_If_Options_Not_Contains_Login_And_Password()
        {
            //Arrange
            var options = new LndHubOptions();

            //Act
            var authentication = LndHubClient.BuildAuthentication(options);

            //Assert
            Assert.IsType<NoAuthentication>(authentication);
        }

    }
}
