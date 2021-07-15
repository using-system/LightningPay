using System.Threading.Tasks;

using LightningPay.Clients.CLightning;

namespace LightningPay.IntegrationTest
{
    public class CLightningClientIntegrationTest : LightningClientIntegrationTestBase
    {
        protected override bool NeedBitcoind => true;

        protected override Task<ILightningClient> GetClient()
        {
            ILightningClient client = CLightningClient.New("tcp://127.0.0.1:48532");

            return Task.FromResult(client);
        }

        protected override string SelfPaymentErrorMesssage => "Error code 210";
    }
}
