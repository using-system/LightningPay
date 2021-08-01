using System;

using Microsoft.Extensions.DependencyInjection;

namespace LightningPay.IntegrationTest
{
    public class LndClientIntegrationTest : LightningClientIntegrationTestBase
    {
        protected override bool NeedBitcoind => true;

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddLndLightningClient(new Uri("http://localhost:32736/"));
        }

        protected override string SelfPaymentErrorMesssage => "self-payments not allowed";
    }
}
