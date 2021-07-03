using System.Net.Http;
using System.Threading.Tasks;

using LightningPay.Infrastructure.Api;

namespace LightningPay.Clients.LNBits
{
    /// <summary>
    ///   LNBits autentication with api key
    /// </summary>
    public class LNBitsAuthentication : AuthenticationBase
    {
        private readonly string apiKey;

        /// <summary>Initializes a new instance of the <see cref="LNBitsAuthentication" /> class.</summary>
        /// <param name="apiKey">The API key.</param>
        public LNBitsAuthentication(string apiKey)
        {
            this.apiKey = apiKey;
        }

        /// <summary>Adds the authentication.</summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public override Task AddAuthentication(HttpClient client, HttpRequestMessage request)
        {
            if(!string.IsNullOrEmpty(this.apiKey))
            {
                request.Headers.Add("X-Api-Key", this.apiKey);
            }

            return Task.CompletedTask;
        }
    }
}
