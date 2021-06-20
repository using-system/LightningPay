using System.Net.Http;

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

        public override void AddAuthentication(HttpRequestMessage request)
        {
            request.Headers.Add("Grpc-Metadata-macaroon", this.macaroon.ToBitString());
        }
    }
}
