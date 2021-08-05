using System;

using Microsoft.Extensions.DependencyInjection;

namespace LightningPay.IntegrationTest
{
    public class LNBitsClientIntegrationTest : LightningClientIntegrationTestBase
    {
        protected override bool NeedBitcoind => false;

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddLNBitsLightningClient(new Uri("https://lnbits.lndev.link/"), "0f920e085f96413fa754b73b9895abbd");
        }

        protected override string SelfPaymentErrorMesssage => "Insufficient balance";
    }
}
