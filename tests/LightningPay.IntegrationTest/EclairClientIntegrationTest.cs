using System.Threading.Tasks;

using LightningPay.Clients.Eclair;

namespace LightningPay.IntegrationTest
{
    public class EclairClientIntegrationTest : LightningClientIntegrationTestBase
    {
        protected override bool NeedBitcoind => true;

        protected override Task<ILightningClient> GetClient()
        {
            ILightningClient client = EclairClient.New("http://localhost:4570/", "eclairpassword");

            return Task.FromResult(client);
        }

        protected override string SelfPaymentErrorMesssage => "self-payments not allowed";
    }
}
