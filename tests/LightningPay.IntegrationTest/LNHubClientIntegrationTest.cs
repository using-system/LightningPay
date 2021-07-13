using System.Threading.Tasks;

using LightningPay.Clients.LndHub;

namespace LightningPay.IntegrationTest
{
    public class LNHubClientIntegrationTest : LightningClientIntegrationTestBase
    {
        protected override bool NeedBitcoind => false;

        protected async override Task<ILightningClient> GetClient()
        {
            ILightningClient client = LndHubClient.New("https://lndhub.herokuapp.com/");

            var wallet = await client.CreateLndHubWallet();

            client = LndHubClient.New("https://lndhub.herokuapp.com/",
                login: wallet.Login,
                password: wallet.Password);

            return client;
        }

        protected override string SelfPaymentErrorMesssage => "not enough balance";
    }

    internal static class LNHubClientIntegrationTestExtensions
    {
        internal async static Task<CreateWalletResponse> CreateLndHubWallet(this ILightningClient client)
        {
            return await client.ToRestClient()
                .Post<CreateWalletResponse>("/create", new CreateWalletRequest()
                {
                    PartnerId = "bluewallet",
                    AccountType = "test"
                });
        }

        internal class CreateWalletRequest
        {
            [Serializable("partnerid")]
            public string PartnerId { get; set; }

            [Serializable("accounttype")]
            public string AccountType { get; set; }
        }

        internal class CreateWalletResponse
        {
            [Serializable("login")]
            public string Login { get; set; }

            [Serializable("password")]
            public string Password { get; set; }
        }
    }
}
