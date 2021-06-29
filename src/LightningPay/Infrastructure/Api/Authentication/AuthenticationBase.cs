using System.Net.Http;
using System.Threading.Tasks;

namespace LightningPay.Infrastructure.Api
{
    /// <summary>
    ///   Authentication base class for api authentication
    /// </summary>
    public abstract class AuthenticationBase
    {
        /// <summary>Adds the authentication.</summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public abstract Task AddAuthentication(HttpClient client, HttpRequestMessage request);
    }
}
