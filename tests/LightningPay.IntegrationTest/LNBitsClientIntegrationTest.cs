using System.Threading.Tasks;

using LightningPay.Clients.LNBits;

namespace LightningPay.IntegrationTest
{
    public class LNBitsClientIntegrationTest : LightningClientIntegrationTestBase
    {
        protected override string SelfPaymentErrorMesssage => "Insufficient balance";

        protected override Task<ILightningClient> GetClient()
        {
            ILightningClient client =
                LNBitsClient.New("https://lnbits.lndev.link/", apiKey: "0f920e085f96413fa754b73b9895abbd");

            return Task.FromResult(client);
        }
    }
}
