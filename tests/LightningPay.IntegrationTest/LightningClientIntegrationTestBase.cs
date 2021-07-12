﻿using NBitcoin;
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
            var balance = await client.GetBalance();
            Assert.True(balance >= 0);

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

        protected async Task WaitServersAreUp(ILightningClient client)
        {
            var rpc = new RPCClient("ceiwHEbqWI83:DwubwWsoo3", "127.0.0.1:37393", Network.RegTest);
            await rpc.ScanRPCCapabilitiesAsync();
            await rpc.GenerateAsync(1);

            while(true)
            {
                try
                {
                    await client.GetBalance();
                    break;
                }
                catch
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
