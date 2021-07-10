using LightningPay.Clients.Lnd;

namespace LightningPay.IntegrationTest
{
    public class LndClientIntegrationTest : LightningClientIntegrationTestBase
    {
        protected override string SelfPaymentErrorMesssage => "self-payments not allowed";

        protected override ILightningClient GetClient()
        {
            return LndClient.New("http://localhost:32736/");
        }
    }
}
