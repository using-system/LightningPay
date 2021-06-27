using System.Net.Http;
using System.Threading.Tasks;

using LightningPay.Infrastructure.Api;

namespace LightningPay.Clients.Lnd
{
    public class MacaroonAuthentication : AuthenticationBase
    {
        private readonly byte[] macaroon;

        public MacaroonAuthentication(byte[] macaroon)
        {
            this.macaroon = macaroon;
        }

        public override Task AddAuthentication(HttpClient client, 
            HttpRequestMessage request)
        {
            if(this.macaroon != null)
            {
                request.Headers.Add("Grpc-Metadata-macaroon", this.macaroon.ToBitString());
            }            

            return Task.CompletedTask;
        }
    }
}
