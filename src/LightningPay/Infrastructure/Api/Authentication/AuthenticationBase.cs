using System.Net.Http;

namespace LightningPay.Infrastructure.Api
{
    public abstract class AuthenticationBase
    {
        public abstract void AddAuthentication(HttpRequestMessage request);
    }
}
