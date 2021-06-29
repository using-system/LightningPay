using System.Net.Http;
using System.Threading.Tasks;

namespace LightningPay.Infrastructure.Api
{
    /// <summary>
    ///   No authentication class
    /// </summary>
    public class NoAuthentication : AuthenticationBase
    {
        /// <summary>Adds the authentication.</summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public override Task AddAuthentication(HttpClient client, HttpRequestMessage request)
        {
            return Task.CompletedTask;
        }
    }
}
