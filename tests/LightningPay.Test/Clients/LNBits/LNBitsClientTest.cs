using System.Threading.Tasks;
using System.Net;
using System;
using System.Net.Http;

using Xunit;

using LightningPay.Clients.LNBits;
using LightningPay.Tools;

namespace LightningPay.Test.Clients.LNBits
{
    public class LNBitsClientTest
    {
        [Fact]
        public async Task CreateInvoice_Should_Return_Lightning_Invoice()
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                (
                    Json.Serialize(new CreateInvoiceResponse()
                    {
                        PaymentRequest = "PaymentRequest",
                        PaymentHash = "Id"
                    }), HttpStatusCode.OK
                ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var lnBitsClient = LNBitsClient.New("https://lnbits.com/api/", "apikey", httpClient);

            //Act
            var invoice = await lnBitsClient.CreateInvoice(1000, "Test");

            //Assert
            Assert.Single(mockMessageHandler.Requests);
            Assert.True(mockMessageHandler.Requests[0].Headers.Contains("X-Api-Key"));
            Assert.Single(mockMessageHandler.Requests[0].Headers.GetValues("X-Api-Key"), "apikey");
            Assert.NotNull(mockMessageHandler.Requests[0].Content);
            var request = Json.Deserialize<CreateInvoiceRequest>(await mockMessageHandler.Requests[0].Content.ReadAsStringAsync());
            Assert.False(request.Out);
            Assert.Equal(1000, request.Amount);
            Assert.Equal("Test", request.Memo);
            Assert.Equal("https://lnbits.com/api/v1/payments", mockMessageHandler.Requests[0].RequestUri.ToString());
            Assert.Equal(1000, invoice.Amount);
            Assert.Equal("Test", invoice.Memo);
            Assert.Equal(LightningInvoiceStatus.Unpaid, invoice.Status);
            Assert.Equal("Id", invoice.Id);
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
            var lnBitsClient = LNBitsClient.New("https://lnbits.com/api/", "apikey", httpClient);

            //Act & Assert
            await Assert.ThrowsAsync<ApiException>(() => lnBitsClient.CreateInvoice(1000, "Test"));
            Assert.Single(mockMessageHandler.Requests);
        }

        [Fact]
        public async Task CreateInvoice_Should_Throw_ApiException_If_Response_Not_Contains_Payment_Request()
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                (
                    Json.Serialize(new CreateInvoiceResponse()
                    {
                    }), HttpStatusCode.OK
                ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var lnBitsClient = LNBitsClient.New("https://lnbits.com/api/", "apikey", httpClient);

            //Act & Assert
            await Assert.ThrowsAsync<ApiException>(() => lnBitsClient.CreateInvoice(1000, "Test"));
            Assert.Single(mockMessageHandler.Requests);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task CheckPayment_Should_Return_Paid_Property(bool paidResponse)
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                 (
                     Json.Serialize(new CheckPaymentResponse()
                     {
                         Paid = paidResponse
                     }), HttpStatusCode.OK
                 ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var lnBitsClient = LNBitsClient.New("https://lnbits.com/api/", "apikey", httpClient);

            //Act
            var actual = await lnBitsClient.CheckPayment("id");

            //Assert
            Assert.True(actual == paidResponse);
            Assert.Single(mockMessageHandler.Requests);
            Assert.Equal("https://lnbits.com/api/v1/payments/id", mockMessageHandler.Requests[0].RequestUri.ToString());
        }

        [Fact]
        public async Task CheckPayment_Should_Throw_ApiException_If_Response_StatusCode_KO()
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
               (
                   "error", HttpStatusCode.BadRequest
               ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var lnBitsClient = LNBitsClient.New("https://lnbits.com/api/", "apikey", httpClient);

            //Act & Assert
            await Assert.ThrowsAsync<ApiException>(() => lnBitsClient.CheckPayment("id"));
            Assert.Single(mockMessageHandler.Requests);
        }

        [Fact]
        public async Task GetBalance_Should_Return_Wallet_Balance()
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                (
                    Json.Serialize(new GetWallletDetailsResponse()
                    {
                        Balance = 2050
                    }), HttpStatusCode.OK
                ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var lnBitsClient = LNBitsClient.New("https://lnbits.com/api/", "apikey", httpClient);

            //Act
            var actual = await lnBitsClient.GetBalance();

            //Assert
            Assert.Equal(2050, actual);
            Assert.Single(mockMessageHandler.Requests);
            Assert.Equal("https://lnbits.com/api/v1/wallet", mockMessageHandler.Requests[0].RequestUri.ToString());
        }

        [Fact]
        public async Task Pay_Should_Return_Payment_Result()
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                (
                    Json.Serialize(new PayResponse()
                    {
                        PaymentHash = "PaymentHash"
                    }), HttpStatusCode.OK
                ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var lnBitsClient = LNBitsClient.New("https://lnbits.com/api/", "apikey", httpClient);

            //Act
            var actual = await lnBitsClient.Pay("request");

            //Assert
            Assert.True(actual);
            Assert.Single(mockMessageHandler.Requests);
            Assert.Equal("https://lnbits.com/api/v1/payments", mockMessageHandler.Requests[0].RequestUri.ToString());
        }

        [Fact]
        public async Task Pay_Should_Throw_ApiException_If_No_Payment_Hash()
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                (
                    Json.Serialize(new PayResponse()
                    {
                        PaymentHash = ""
                    }), HttpStatusCode.OK
                ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var lnBitsClient = LNBitsClient.New("https://lnbits.com/api/", "apikey", httpClient);

            //Act
            await Assert.ThrowsAsync<ApiException>(() => lnBitsClient.Pay("request"));

            //Assert
            Assert.Single(mockMessageHandler.Requests);
            Assert.Equal("https://lnbits.com/api/v1/payments", mockMessageHandler.Requests[0].RequestUri.ToString());
        }

        [Fact]
        public void BuildAuthentication_Should_Return_LndHubAuthentication_If_Options_Contains_ApiKey()
        {
            //Arrange
            var options = new LNBitsOptions()
            {
                ApiKey = "apikey"
            };

            //Act
            var authentication = LNBitsClient.BuildAuthentication(options);

            //Assert
            Assert.IsType<LNBitsAuthentication>(authentication);
        }

        [Fact]
        public void BuildAuthentication_Should_Throw_ArgumentException_If_Options_Not_Contains_ApiKey()
        {
            //Arrange
            var options = new LNBitsOptions();

            //Act && Assert
            Assert.Throws<ArgumentException>(() => LNBitsClient.BuildAuthentication(options));
        }
    }
}
