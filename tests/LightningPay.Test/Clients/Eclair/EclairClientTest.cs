using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

using Xunit;

using LightningPay.Clients.Eclair;
using LightningPay.Tools;

namespace LightningPay.Test.Clients.Eclair
{
    public class EclairClientTest
    {
        [Fact]
        public async Task CheckConnectivity_Should_Return_Ok_If_NodeId_NotEmpty()
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                (
                    Json.Serialize(new GetInfoResponse()
                    {
                        NodeId = "nodeid"
                    }), HttpStatusCode.OK
                ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var eclairClient = EclairClient.New("https://localhost:8080", "password", httpClient);

            //Act
            var actual = await eclairClient.CheckConnectivity();

            //Assert
            Assert.Single(mockMessageHandler.Requests);
            Assert.Equal("https://localhost:8080/getinfo", mockMessageHandler.Requests[0].RequestUri.ToString());
            Assert.NotNull(mockMessageHandler.Requests[0].Headers.Authorization);
            Assert.Equal(CheckConnectivityResult.Ok, actual.Result);
            Assert.Null(actual.Error);
        }

        [Fact]
        public async Task CheckConnectivity_Should_Return_Error_If_WalletId_Empty()
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                (
                    Json.Serialize(new GetInfoResponse()
                    {
                    }), HttpStatusCode.OK
                ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var eclairClient = EclairClient.New("https://localhost:8080", "password", httpClient);

            //Act
            var actual = await eclairClient.CheckConnectivity();

            //Assert
            Assert.Single(mockMessageHandler.Requests);
            Assert.Equal("https://localhost:8080/getinfo", mockMessageHandler.Requests[0].RequestUri.ToString());
            Assert.NotNull(mockMessageHandler.Requests[0].Headers.Authorization);
            Assert.Equal(CheckConnectivityResult.Error, actual.Result);
            Assert.NotNull(actual.Error);
        }

        [Fact]
        public async Task CheckConnectivity_Should_Return_Error_If_Http_Error()
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                (
                    Json.Serialize(new GetInfoResponse()
                    {
                    }), HttpStatusCode.ServiceUnavailable
                ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var eclairClient = EclairClient.New("https://localhost:8080", "password", httpClient);

            //Act
            var actual = await eclairClient.CheckConnectivity();

            //Assert
            Assert.Single(mockMessageHandler.Requests);
            Assert.Equal("https://localhost:8080/getinfo", mockMessageHandler.Requests[0].RequestUri.ToString());
            Assert.NotNull(mockMessageHandler.Requests[0].Headers.Authorization);
            Assert.Equal(CheckConnectivityResult.Error, actual.Result);
            Assert.NotNull(actual.Error);
        }

        [Fact]
        public async Task CreateInvoice_Should_Return_Lightning_Invoice()
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                (
                    Json.Serialize(new CreateInvoiceResponse()
                    {
                        PaymentHash = "Id",
                        Description = "Test",
                        Amount = 1000000,
                        Serialized = "PaymentRequest",
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        Expiry = 60 * 60 * 24
                    }), HttpStatusCode.OK
                ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var eclairClient = EclairClient.New("https://localhost:8080", "password", httpClient);

            //Act
            var invoice = await eclairClient.CreateInvoice(1000, "Test");

            //Assert
            Assert.Single(mockMessageHandler.Requests);
            Assert.Equal("https://localhost:8080/createinvoice", mockMessageHandler.Requests[0].RequestUri.ToString());
            Assert.NotNull(mockMessageHandler.Requests[0].Headers.Authorization);
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
            var eclairClient = EclairClient.New("https://localhost:8080", "password", httpClient);

            //Act & Assert
            await Assert.ThrowsAsync<LightningPayException>(() => eclairClient.CreateInvoice(1000, "Test"));
            Assert.Single(mockMessageHandler.Requests);
        }

        [Fact]
        public async Task CheckPayment_Should_Return_True_If_Status_Ok()
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                 (
                     Json.Serialize(new GetReceivedInfoResponse()
                     {
                         Amount = 1000,
                         Status = new GetReceivedInfoResponse.GetReceivedInfoStatusResponse()
                         {
                             Type = "received",
                             Amount = 1000
                         }
                     }), HttpStatusCode.OK
                 ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var eclairClient = EclairClient.New("https://localhost:8080", "password", httpClient);

            //Act
            var actual = await eclairClient.CheckPayment("id");

            //Assert
            Assert.True(actual);
            Assert.Single(mockMessageHandler.Requests);
            Assert.Equal("https://localhost:8080/getreceivedinfo", mockMessageHandler.Requests[0].RequestUri.ToString());
        }

        [Fact]
        public async Task CheckPayment_Should_Return_False_If_Status_Pending()
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                 (
                     Json.Serialize(new GetReceivedInfoResponse()
                     {
                         Amount = 1000,
                         Status = new GetReceivedInfoResponse.GetReceivedInfoStatusResponse()
                         {
                             Type = "pending",
                             Amount = 1000
                         }
                     }), HttpStatusCode.OK
                 ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var eclairClient = EclairClient.New("https://localhost:8080", "password", httpClient);

            //Act
            var actual = await eclairClient.CheckPayment("id");

            //Assert
            Assert.False(actual);
            Assert.Single(mockMessageHandler.Requests);
            Assert.Equal("https://localhost:8080/getreceivedinfo", mockMessageHandler.Requests[0].RequestUri.ToString());
        }

        [Fact]
        public async Task CheckPayment_Should_Return_False_If_TotalAmount_Not_Receive()
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                 (
                     Json.Serialize(new GetReceivedInfoResponse()
                     {
                         Amount = 1000,
                         Status = new GetReceivedInfoResponse.GetReceivedInfoStatusResponse()
                         {
                             Type = "received",
                             Amount = 500
                         }
                     }), HttpStatusCode.OK
                 ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var eclairClient = EclairClient.New("https://localhost:8080", "password", httpClient);

            //Act
            var actual = await eclairClient.CheckPayment("id");

            //Assert
            Assert.False(actual);
            Assert.Single(mockMessageHandler.Requests);
            Assert.Equal("https://localhost:8080/getreceivedinfo", mockMessageHandler.Requests[0].RequestUri.ToString());
        }

        [Fact]
        public async Task GetBalance_Should_Return_OnChain_Balance()
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                (
                    Json.Serialize(new GetBalanceResponse()
                    {
                        Confirmed = 5000
                    }), HttpStatusCode.OK
                ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var eclairClient = EclairClient.New("https://localhost:8080", "password", httpClient);

            //Act
            var actual = await eclairClient.GetBalance();

            //Assert
            Assert.Equal(5000, actual);
            Assert.Single(mockMessageHandler.Requests);
            Assert.Equal("https://localhost:8080/onchainbalance", mockMessageHandler.Requests[0].RequestUri.ToString());
        }

        [Fact]
        public async Task Pay_Should_Return_Payment_Result()
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                (
                    Json.Serialize("id"), HttpStatusCode.OK
                ),
                (
                    Json.Serialize(new GetSentInfoResponse()
                    {
                        new SentInfoResponse()
                        {
                            Amount = 1000,
                            Status = new SentInfoResponse.GetReceivedInfoStatusResponse()
                            {
                                Type = "sent"
                            }
                        }
                    }), HttpStatusCode.OK
                ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var eclairClient = EclairClient.New("https://localhost:8080", "password", httpClient);

            //Act
            var actual = await eclairClient.Pay("request");

            //Assert
            Assert.Equal(PayResult.Ok, actual.Result);
            Assert.Equal(2, mockMessageHandler.Requests.Count);
            Assert.Equal("https://localhost:8080/payinvoice", mockMessageHandler.Requests[0].RequestUri.ToString());
            Assert.Equal("https://localhost:8080/getsentinfo", mockMessageHandler.Requests[1].RequestUri.ToString());
        }

        [Fact]
        public async Task Pay_Should_Return_Payment_Result_While_Status_Not_Sent()
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                (
                    Json.Serialize("id"), HttpStatusCode.OK
                ),
                (
                    Json.Serialize(new GetSentInfoResponse()
                    {
                        new SentInfoResponse()
                        {
                            Amount = 1000,
                            Status = new SentInfoResponse.GetReceivedInfoStatusResponse()
                            {
                                Type = "pending"
                            }
                        }
                    }), HttpStatusCode.OK
                ),
                (
                    Json.Serialize(new GetSentInfoResponse()
                    {
                        new SentInfoResponse()
                        {
                            Amount = 1000,
                            Status = new SentInfoResponse.GetReceivedInfoStatusResponse()
                            {
                                Type = "sent"
                            }
                        }
                    }), HttpStatusCode.OK
                ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var eclairClient = EclairClient.New("https://localhost:8080", "password", httpClient);

            //Act
            var actual = await eclairClient.Pay("request");

            //Assert
            Assert.Equal(PayResult.Ok, actual.Result);
            Assert.Equal(3, mockMessageHandler.Requests.Count);
            Assert.Equal("https://localhost:8080/payinvoice", mockMessageHandler.Requests[0].RequestUri.ToString());
            Assert.Equal("https://localhost:8080/getsentinfo", mockMessageHandler.Requests[1].RequestUri.ToString());
            Assert.Equal("https://localhost:8080/getsentinfo", mockMessageHandler.Requests[2].RequestUri.ToString());
        }

        [Fact]
        public async Task Pay_Should_Return_Error_If_Status_Failed()
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                (
                    Json.Serialize("id"), HttpStatusCode.OK
                ),
                (
                    Json.Serialize(new GetSentInfoResponse()
                    {
                        new SentInfoResponse()
                        {
                            Amount = 1000,
                            Status = new SentInfoResponse.GetReceivedInfoStatusResponse()
                            {
                                Type = "failed"
                            }
                        }
                    }), HttpStatusCode.OK
                ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var eclairClient = EclairClient.New("https://localhost:8080", "password", httpClient);

            //Act
            var actual = await eclairClient.Pay("request");

            //Assert
            Assert.Equal(PayResult.Error, actual.Result);
            Assert.NotNull(actual.Error);
            Assert.Equal(2, mockMessageHandler.Requests.Count);
            Assert.Equal("https://localhost:8080/payinvoice", mockMessageHandler.Requests[0].RequestUri.ToString());
            Assert.Equal("https://localhost:8080/getsentinfo", mockMessageHandler.Requests[1].RequestUri.ToString());
        }

        [Fact]
        public async Task Pay_Should_Return_Error_If_Status_Unkown()
        {
            //Arrange
            var mockMessageHandler = new MockHttpMessageHandler(
                (
                    Json.Serialize("id"), HttpStatusCode.OK
                ),
                (
                    Json.Serialize(new GetSentInfoResponse()
                    {
                        new SentInfoResponse()
                        {
                            Amount = 1000,
                            Status = new SentInfoResponse.GetReceivedInfoStatusResponse()
                            {
                                Type = "unkown"
                            }
                        }
                    }), HttpStatusCode.OK
                ));

            HttpClient httpClient = new HttpClient(mockMessageHandler);
            var eclairClient = EclairClient.New("https://localhost:8080", "password", httpClient);

            //Act
            var actual = await eclairClient.Pay("request");

            //Assert
            Assert.Equal(PayResult.Error, actual.Result);
            Assert.NotNull(actual.Error);
            Assert.Equal(2, mockMessageHandler.Requests.Count);
            Assert.Equal("https://localhost:8080/payinvoice", mockMessageHandler.Requests[0].RequestUri.ToString());
            Assert.Equal("https://localhost:8080/getsentinfo", mockMessageHandler.Requests[1].RequestUri.ToString());
        }

        [Fact]
        public void BuildAuthentication_Should_Return_EclairAuthentication_If_Options_Contains_Password()
        {
            //Arrange
            var options = new EclairOptions()
            {
                Password = "password"
            };

            //Act
            var authentication = EclairClient.BuildAuthentication(options);

            //Assert
            Assert.IsType<EclairAuthentication>(authentication);
        }

        [Fact]
        public void BuildAuthentication_Should_Throw_ArgumentException_If_Options_Not_Contains_Passwword()
        {
            //Arrange
            var options = new EclairOptions();

            //Act && Assert
            Assert.Throws<ArgumentException>(() => EclairClient.BuildAuthentication(options));
        }
    }
}
