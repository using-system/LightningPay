using System;

using Microsoft.Extensions.DependencyInjection;

namespace LightningPay.IntegrationTest
{
    public class LNHubClientIntegrationTest : LightningClientIntegrationTestBase
    {
        protected override bool NeedBitcoind => false;

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddLndHubLightningClient(new Uri("https://lndhub.herokuapp.com/"), "2073282b83fad2955b57", "a1c4f8c30a93bf3e8cbf");
        }

        protected override string SelfPaymentErrorMesssage => "not enough balance";
    }

}
