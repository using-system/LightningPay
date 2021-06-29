using System.Net.Http;
using System.Threading.Tasks;

using LightningPay.Infrastructure.Api;

namespace LightningPay.Clients.Lnd
{
    /// <summary>
    ///   LND Authentication with Macaroon
    /// </summary>
    public class MacaroonAuthentication : AuthenticationBase
    {
        private readonly byte[] macaroon;

        /// <summary>Initializes a new instance of the <see cref="MacaroonAuthentication" /> class.</summary>
        /// <param name="macaroon">The macaroon.</param>
        public MacaroonAuthentication(byte[] macaroon)
        {
            this.macaroon = macaroon;
        }

        /// <summary>Adds the authentication.</summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <br />
        /// </returns>
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
