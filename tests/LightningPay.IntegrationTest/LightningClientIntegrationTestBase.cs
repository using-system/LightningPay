using System;
using System.Threading.Tasks;

using Xunit;

namespace LightningPay.IntegrationTest
{
    public abstract class LightningClientIntegrationTestBase
    {
        [Fact]
        public async Task Run()
        {
            var client = this.GetClient();

            //Get balance for the first time
            var balance = await client.GetBalance();
            Assert.Equal(0, balance);

            //Create an invoice
            var invoice = await client.CreateInvoice(1000, "Test invoice");
            Assert.NotNull(invoice);
            Assert.NotNull(invoice.Id);
            Assert.NotNull(invoice.BOLT11);
            Assert.Equal(1000, invoice.Amount);
            Assert.Equal("Test invoice", invoice.Memo);

            //Check payment (not paid)
            var isPaid = await client.CheckPayment(invoice.Id);
            Assert.False(isPaid);

            //Self payment is not allowed
            try
            {
                await client.Pay(invoice.BOLT11);
                throw new Exception("self-payments not allowed");
            }
            catch(LightningPayException exc)
            {
                Assert.Equal(LightningPayException.ErrorCode.BAD_REQUEST, exc.Code);
                Assert.Contains(this.SelfPaymentErrorMesssage, exc.Message);
            }
            catch (Exception)
            {
                throw new Exception("self-payments not allowed");
            }
        }

        protected abstract ILightningClient GetClient();

        protected abstract string SelfPaymentErrorMesssage { get; }
    }
}
