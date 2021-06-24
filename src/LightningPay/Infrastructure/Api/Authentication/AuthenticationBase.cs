using System.Net.Http;
using System.Threading.Tasks;

namespace LightningPay.Infrastructure.Api
{
    public abstract class AuthenticationBase
    {
        public abstract Task AddAuthentication(HttpClient client, HttpRequestMessage request);
    }
}
