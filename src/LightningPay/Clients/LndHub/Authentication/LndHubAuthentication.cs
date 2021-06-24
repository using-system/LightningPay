using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using LightningPay.Infrastructure.Api;
using LightningPay.Tools;

namespace LightningPay.Clients.LndHub
{
    public class LndHubAuthentication : AuthenticationBase
    {
        private readonly LndHubOptions options;
        public LndHubAuthentication(LndHubOptions options)
        {
            this.options = options;
        }

        public async override Task AddAuthentication(HttpClient client, 
            HttpRequestMessage request)
        {
            try
            {
                var response = await client.PostAsync($"{this.options.BaseUri.ToBaseUrl()}/auth?type=auth",
                    new StringContent(Json.Serialize(new GetTokenRequest()
                    {
                        Login = options.Login,
                        Password = options.Password
                    }), Encoding.UTF8, "application/json"));

                if(response.IsSuccessStatusCode)
                {
                    var tokenResponse = Json.Deserialize<GetTokenResponse>(await response.Content.ReadAsStringAsync());
                    if(string.IsNullOrEmpty(tokenResponse.AccessToken))
                    {
                        throw new ApiException(
                            $"Bad Autentication to the lndhub : {this.options.BaseUri}",
                            HttpStatusCode.Unauthorized);
                    }

                    request.Headers.Add("Authorization", $"Bearer {tokenResponse.AccessToken}");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    throw new ApiException(
                        $"Bad Autentication to the lndhub : {this.options.BaseUri} with error status code {response.StatusCode} and response {errorContent}",
                        HttpStatusCode.Unauthorized);
                }
            }
            catch(ApiException)
            {
                throw;
            }
            catch(Exception exc)
            {
                throw new ApiException($"Bad Autentication",
                   HttpStatusCode.Unauthorized,
                   innerException: exc);
            }

        }
    }
}
