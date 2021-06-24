using System.Net.Http;
using System.Threading.Tasks;

namespace LightningPay.Infrastructure.Api
{
    public class NoAuthentication : AuthenticationBase
    {
        public override Task AddAuthentication(HttpClient client, HttpRequestMessage request)
        {
            return Task.CompletedTask;
        }
    }
}
