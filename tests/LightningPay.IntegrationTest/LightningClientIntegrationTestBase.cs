using NBitcoin;
using NBitcoin.RPC;
using System;
using System.Threading.Tasks;

using Xunit;

namespace LightningPay.IntegrationTest
{
    public abstract class LightningClientIntegrationTestBase
    {
        [Fact(Timeout =(2 * 60 * 1000))]
        public async Task Run()
        {
            var client = await this.GetClient();

            await this.WaitServersAreUp(client);

            //Get balance for the first time
            if (!(client is Clients.Eclair.EclairClient))
            {
                var balance = await client.GetBalance();
                Assert.True(balance.MilliSatoshis >= 0);
            }


            //Create an invoice
            var invoice = await client.CreateInvoice(Money.FromSatoshis(1000), "Test invoice");
            Assert.NotNull(invoice);
            Assert.NotNull(invoice.Id);
            Assert.NotNull(invoice.BOLT11);
            Assert.Equal(1000, invoice.Amount.ToSatoshis());
            Assert.Equal("Test invoice", invoice.Memo);

            //Check payment (not paid)
            var isPaid = await client.CheckPayment(invoice.Id);
            Assert.False(isPaid);

            //Self payment is not allowed
            var response = await client.Pay(invoice.BOLT11);
            Assert.Equal(PayResult.Error, response.Result);
            Assert.Contains(this.SelfPaymentErrorMesssage, response.Error);
        }

        protected async Task WaitServersAreUp(ILightningClient client)
        {
            var rpc = new RPCClient("ceiwHEbqWI83:DwubwWsoo3", "127.0.0.1:37393", Network.RegTest);
            await rpc.ScanRPCCapabilitiesAsync();
            await rpc.GenerateAsync(1);

            while(true)
            {
                var checkConnectivty = await client.CheckConnectivity();
                if(checkConnectivty.Result == CheckConnectivityResult.Ok)
                {
                    break;
                }
                else
                {
                    await Task.Delay(5000);
                    continue;
                }
            }
        }

        protected abstract bool NeedBitcoind { get; }

        protected abstract Task<ILightningClient> GetClient();

        protected abstract string SelfPaymentErrorMesssage { get; }

    }
}
