using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

using LightningPay.Clients.CLightning;
using LightningPay.Tools;

using NSubstitute;
using Xunit;

namespace LightningPay.Test.Clients.CLightning
{
    public class CLightningClientTest
    {
        [Fact]
        public async Task CheckConnectivity_Should_Return_Ok_If_NodeAlias_NotEmpty()
        {
            //Arrange
            IRpcClient rpcClient = Substitute.For<IRpcClient>();
            rpcClient.SendCommandAsync<GetInfoResponse>("getinfo")
                .Returns(new GetInfoResponse() { Id = "Id" });
            var cLightningClient = new CLightningClient(rpcClient);

            //Act
            var actual = await cLightningClient.CheckConnectivity();

            //Assert
            Assert.Equal(CheckConnectivityResult.Ok, actual.Result);
            Assert.Null(actual.Error);
        }

        [Fact]
        public async Task CheckConnectivity_Should_Return_Error_If_NodeAlias_Empty()
        {
            //Arrange
            IRpcClient rpcClient = Substitute.For<IRpcClient>();
            rpcClient.SendCommandAsync<GetInfoResponse>("getinfo")
                .Returns(new GetInfoResponse());
            var cLightningClient = new CLightningClient(rpcClient);

            //Act
            var actual = await cLightningClient.CheckConnectivity();

            //Assert
            Assert.Equal(CheckConnectivityResult.Error, actual.Result);
            Assert.NotNull(actual.Error);
        }

        [Fact]
        public async Task CheckConnectivity_Should_Return_Error_On_Exception()
        {
            //Arrange
            IRpcClient rpcClient = Substitute.For<IRpcClient>();
            rpcClient.When(c => c.SendCommandAsync<GetInfoResponse>("getinfo"))
                .Do(c => throw new Exception("an exception occurs"));
            var cLightningClient = new CLightningClient(rpcClient);

            //Act
            var actual = await cLightningClient.CheckConnectivity();

            //Assert
            Assert.Equal(CheckConnectivityResult.Error, actual.Result);
            Assert.NotNull(actual.Error);
        }

        [Fact]
        public async Task CreateInvoice_Should_Return_Lightning_Invoice()
        {
            //Arrange
            IRpcClient rpcClient = Substitute.For<IRpcClient>();
            rpcClient.SendCommandAsync<CLightningInvoice>("invoice", (long)1000000, Arg.Any<string>(), "Test", "86400")
                .Returns(new CLightningInvoice() 
                {
                    Label = "0001",
                    Description = "Test",
                    MilliSatoshi = Money.FromSatoshis(1000).MilliSatoshis,
                    BOLT11 = "PaymentRequest",
                    Status = "unpaid",
                    ExpiryAt = DateTimeOffset.Now.AddHours(24)
                });
            var cLightningClient = new CLightningClient(rpcClient);

            //Act
            var invoice = await cLightningClient.CreateInvoice(Money.FromSatoshis(1000), "Test");

            //Assert
            Assert.Equal(1000, invoice.Amount.ToSatoshis());
            Assert.Equal("Test", invoice.Memo);
            Assert.Equal(LightningInvoiceStatus.Unpaid, invoice.Status);
            Assert.NotNull(invoice.Id);
            Assert.Equal("PaymentRequest", invoice.BOLT11);
            Assert.True(invoice.ExpiresAt > DateTimeOffset.UtcNow.AddHours(23));
        }

        [Fact]
        public async Task CheckPayment_Should_Return_True_On_Status_Paid()
        {
            //Arrange
            IRpcClient rpcClient = Substitute.For<IRpcClient>();
            rpcClient.SendCommandAsync<ListInvoicesResponse>("listinvoices", "id")
                .Returns(new ListInvoicesResponse()
                {
                   Invoices =
                    {
                        new CLightningInvoice()
                        {
                            Label = "id",
                            MilliSatoshi = 1000000,
                            Status =  "paid"
                        }
                    }
                });
            var cLightningClient = new CLightningClient(rpcClient);

            //Act
            var actual = await cLightningClient.CheckPayment("id");

            //Assert
            Assert.True(actual);
        }

        [Fact]
        public async Task CheckPayment_Should_Return_False_On_Status_Unpaid()
        {
            //Arrange
            IRpcClient rpcClient = Substitute.For<IRpcClient>();
            rpcClient.SendCommandAsync<ListInvoicesResponse>("listinvoices", "id")
                .Returns(new ListInvoicesResponse()
                {
                    Invoices =
                    {
                        new CLightningInvoice()
                        {
                            Label = "id",
                            MilliSatoshi = 1000000,
                            Status =  "unpaid"
                        }
                    }
                });
            var cLightningClient = new CLightningClient(rpcClient);

            //Act
            var actual = await cLightningClient.CheckPayment("id");

            //Assert
            Assert.False(actual);
        }

        [Fact]
        public async Task CheckPayment_Should_Return_False_If_No_Invoice()
        {
            //Arrange
            IRpcClient rpcClient = Substitute.For<IRpcClient>();
            rpcClient.SendCommandAsync<ListInvoicesResponse>("listinvoices", "id")
                .Returns(new ListInvoicesResponse());
            var cLightningClient = new CLightningClient(rpcClient);

            //Act
            var actual = await cLightningClient.CheckPayment("id");

            //Assert
            Assert.False(actual);
        }

        [Fact]
        public async Task GetBalance_Should_Return_Confirmed_Funds_Outputs()
        {
            //Arrange
            IRpcClient rpcClient = Substitute.For<IRpcClient>();
            rpcClient.SendCommandAsync<ListFundsResponse>("listfunds")
                .Returns(new ListFundsResponse()
                {
                    Outputs =
                    {
                        new ListFundsResponse.Output(){Value = 2500, Status = "confirmed"},
                        new ListFundsResponse.Output(){Value = 2000 },
                        new ListFundsResponse.Output(){Value = 1000, Status = "unconfirmed" },
                        new ListFundsResponse.Output(){Value = 50, Status = "confirmed" }
                    }
                });
            var cLightningClient = new CLightningClient(rpcClient);

            //Act
            var actual = await cLightningClient.GetBalance();

            //Assert
            Assert.Equal(2550, actual.ToSatoshis());
        }

        [Fact]
        public async Task Pay_Should_Return_Payment_Result()
        {
            //Arrange
            IRpcClient rpcClient = Substitute.For<IRpcClient>();
            rpcClient.SendCommandAsync<object>("pay", "request")
                .Returns(new object());
            var cLightningClient = new CLightningClient(rpcClient);

            //Act
            var actual = await cLightningClient.Pay("request");

            //Assert
            Assert.Equal(PayResult.Ok, actual.Result);
        }

        [Fact]
        public async Task Pay_Should_Return_Error_If_An_Payment_Error_Occurs()
        {
            //Arrange
            IRpcClient rpcClient = Substitute.For<IRpcClient>();
            rpcClient.When(c => c.SendCommandAsync<object>("pay", "request"))
                .Do(c => throw new LightningPayException("an exception occurs"));
            var cLightningClient = new CLightningClient(rpcClient);

            //Act
            var actual = await cLightningClient.Pay("request");

            //Assert
            Assert.Equal(PayResult.Error, actual.Result);
        }
    }
}
