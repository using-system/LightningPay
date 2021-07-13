using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using LightningPay.Infrastructure.Api;

namespace LightningPay.Clients.Eclair
{
    /// <summary>
    ///   Eclair authentication with password
    /// </summary>
    public class EclairAuthentication : AuthenticationBase
    {
        internal const string HEADER_KEY = "Basic";

        private readonly string password;

        /// <summary>Initializes a new instance of the <see cref="EclairAuthentication" /> class.</summary>
        /// <param name="password">The password.</param>
        public EclairAuthentication(string password)
        {
            this.password = password;
        }

        /// <summary>Adds the authentication.</summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public override Task AddAuthentication(HttpClient client, HttpRequestMessage request)
        {
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(this.password))
            {
                request.Headers.Authorization = 
                    new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.Default.GetBytes($":{this.password}")));
            }

            return Task.CompletedTask;
        }
    }
}
