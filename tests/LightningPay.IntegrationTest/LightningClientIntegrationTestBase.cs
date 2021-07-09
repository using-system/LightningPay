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
        }

        protected abstract ILightningClient GetClient();
    }
}
