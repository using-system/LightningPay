using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

using LightningPay.Core;
using LightningPay.Core.Clients.Lnd;

namespace LightningPay.Samples.Console
{
    class LndClientSample : SampleBase
    {
        public async override Task Execute()
        {
            var client = new LndClient(new System.Net.Http.HttpClient(), new LndOptionsWrapper(new LndOptions()
            {
                BaseUri = new Uri("http://localhost:42802/")
            }));

            var invoice = await client.CreateInvoice(LightMoney.MilliSatoshis(1000), "Test", TimeSpan.FromMinutes(5));

            System.Console.WriteLine(invoice.Id);
        }

        public class LndOptionsWrapper : IOptions<LndOptions>
        {
            private readonly LndOptions options;

            public LndOptionsWrapper(LndOptions options)
            {
                this.options = options;
            }

            public LndOptions Value => this.options;
        }
    }
}
