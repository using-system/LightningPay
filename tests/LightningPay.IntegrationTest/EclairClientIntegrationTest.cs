using System;

using Microsoft.Extensions.DependencyInjection;

namespace LightningPay.IntegrationTest
{
    public class EclairClientIntegrationTest : LightningClientIntegrationTestBase
    {
        protected override bool NeedBitcoind => true;

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddEclairLightningClient(new Uri("http://localhost:4570/"), "eclairpassword");
        }

        protected override string SelfPaymentErrorMesssage => "Cannot process to the payment";
    }
}
