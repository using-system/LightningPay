using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Xunit;

using LightningPay.Clients.Lnd;
using LightningPay.Tools;
using LightningPay.Infrastructure.Api;

namespace LightningPay.Test.Clients.Lnd
{
    public class LndClientTest
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task CreateInvoice_Should_Return_Lightning_Invoice(bool withMacaroon)
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                (
                    Json.Serialize(new AddInvoiceResponse()
                    {
                        Payment_request = "PaymentRequest",
                        R_hash = new byte[] { 0, 1 }
                    }), HttpStatusCode.OK
                ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var lndClient = LndClient.New("http://localhost:42802/", withMacaroon ? "971a5512" : null, httpClient: httpClient);

            //Act
            var invoice = await lndClient.CreateInvoice(1000, "Test");

            //Assert
            Assert.Single(mockMessageHandler.Requests);
            Assert.True(mockMessageHandler.Requests[0].Headers.Contains("Grpc-Metadata-macaroon") == withMacaroon);
            Assert.Equal("http://localhost:42802/v1/invoices", mockMessageHandler.Requests[0].RequestUri.ToString());
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
            var lndClient = LndClient.New("http://localhost:42802/", httpClient: httpClient);

            //Act & Assert
            await Assert.ThrowsAsync<ApiException>(() => lndClient.CreateInvoice(1000, "Test"));
            Assert.Single(mockMessageHandler.Requests);
        }

        [Fact]
        public async Task CreateInvoice_Should_Throw_ApiException_If_Response_Not_Contains_Payment_Request()
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                (
                    Json.Serialize(new AddInvoiceResponse()), HttpStatusCode.OK
                ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var lndClient = LndClient.New("http://localhost:42802/", httpClient: httpClient);

            //Act & Assert
            await Assert.ThrowsAsync<ApiException>(() => lndClient.CreateInvoice(1000, "Test"));
            Assert.Single(mockMessageHandler.Requests);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        public async Task CheckPayment_Should_Return_Settled_Property(bool withMacaroon, bool settled, bool expectedResult)
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                (
                    Json.Serialize(new LnrpcInvoice()
                    {
                        Payment_request = "PaymentRequest",
                        R_hash = new byte[] { 0, 1 },
                        Settled = settled,
                        Creation_date = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                        Expiry = 3600.ToString()
                    }), HttpStatusCode.OK
                ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var lndClient = LndClient.New("http://localhost:42802/", withMacaroon ? "971a5512" : null, httpClient: httpClient);

            //Act
            var actual = await lndClient.CheckPayment("id");

            //Assert
            Assert.True(actual == expectedResult);
            Assert.Single(mockMessageHandler.Requests);
            Assert.True(mockMessageHandler.Requests[0].Headers.Contains("Grpc-Metadata-macaroon") == withMacaroon);
            Assert.Equal("http://localhost:42802/v1/invoice/id", mockMessageHandler.Requests[0].RequestUri.ToString());
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
            var lndClient = LndClient.New("http://localhost:42802/", httpClient: httpClient);

            //Act & Assert
            await Assert.ThrowsAsync<ApiException>(() => lndClient.CheckPayment("id"));
            Assert.Single(mockMessageHandler.Requests);
        }

        [Fact]
        public async Task GetBalance_Should_Return_Wallet_Balance()
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                (
                    Json.Serialize(new GetBalanceResponse()
                    {
                        TotalBalance = 5000
                    }), HttpStatusCode.OK
                ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var lndClient = LndClient.New("http://localhost:42802/", null, httpClient: httpClient);

            //Act
            var actual = await lndClient.GetBalance();

            //Assert
            Assert.Equal(5000, actual);
            Assert.Single(mockMessageHandler.Requests);
            Assert.Equal("http://localhost:42802/v1/balance/blockchain", mockMessageHandler.Requests[0].RequestUri.ToString());
        }

        [Fact]
        public void BuildAuthentication_Should_Return_NoAuthentication_If_Options_Not_Contains_Macaroon()
        {
            //Arrange
            var options = new LndOptions();

            //Act
            var authentication = LndClient.BuildAuthentication(options);

            //Assert
            Assert.IsType<NoAuthentication>(authentication);
        }

        [Fact]
        public void BuildAuthentication_Should_Return_MacaroonAuthentication_If_Options_Contains_Macaroon()
        {
            //Arrange
            var options = new LndOptions()
            {
                Macaroon = new byte[] { 0, 1 }
            };

            //Act
            var authentication = LndClient.BuildAuthentication(options);

            //Assert
            Assert.IsType<MacaroonAuthentication>(authentication);
        }
    }
}
