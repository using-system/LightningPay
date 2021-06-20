using System.Net.Http;

namespace LightningPay.Infrastructure.Api
{
    public class NoAuthentication : AuthenticationBase
    {
        public override void AddAuthentication(HttpRequestMessage request)
        {
           
        }
    }
}
