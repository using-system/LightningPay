using System.Threading.Tasks;

using LightningPay.Clients.Lnd;

namespace LightningPay.IntegrationTest
{
    public class LndClientIntegrationTest : LightningClientIntegrationTestBase
    {
        protected override string SelfPaymentErrorMesssage => "self-payments not allowed";

        protected override Task<ILightningClient> GetClient()
        {
            ILightningClient client = LndClient.New("http://localhost:32736/");

            return Task.FromResult(client);
        }
    }
}
