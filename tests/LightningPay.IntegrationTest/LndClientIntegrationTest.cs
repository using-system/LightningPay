using LightningPay.Clients.Lnd;

namespace LightningPay.IntegrationTest
{
    public class LndClientIntegrationTest : LightningClientIntegrationTestBase
    {
        protected override ILightningClient GetClient()
        {
            return LndClient.New("http://localhost:32736/");
        }
    }
}
