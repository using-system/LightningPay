using System;

using Microsoft.Extensions.DependencyInjection;

namespace LightningPay.IntegrationTest
{
    public class CLightningClientIntegrationTest : LightningClientIntegrationTestBase
    {
        protected override bool NeedBitcoind => true;


        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddCLightningClient(new Uri("tcp://127.0.0.1:48532"));
        }

        protected override string SelfPaymentErrorMesssage => "Error code 210";
    }
}
