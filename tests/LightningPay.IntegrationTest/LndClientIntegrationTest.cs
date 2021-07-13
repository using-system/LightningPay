using System.Threading.Tasks;

using LightningPay.Clients.Lnd;

namespace LightningPay.IntegrationTest
{
    public class LndClientIntegrationTest : LightningClientIntegrationTestBase
    {
        protected override bool NeedBitcoind => true;

        protected override Task<ILightningClient> GetClient()
        {
            ILightningClient client = LndClient.New("http://localhost:32736/");

            return Task.FromResult(client);
        }

        protected override string SelfPaymentErrorMesssage => "self-payments not allowed";
    }
}
