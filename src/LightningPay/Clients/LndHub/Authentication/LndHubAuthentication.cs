using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using LightningPay.Infrastructure.Api;
using LightningPay.Tools;

namespace LightningPay.Clients.LndHub
{
    /// <summary>
    ///   LNDHub authentication
    /// </summary>
    public class LndHubAuthentication : AuthenticationBase
    {
        private readonly LndHubOptions options;

        private (string AccessToken, string RefreshToken, DateTimeOffset CreationDate) token;

        /// <summary>Initializes a new instance of the <see cref="LndHubAuthentication" /> class.</summary>
        /// <param name="options">The options.</param>
        public LndHubAuthentication(LndHubOptions options)
        {
            this.options = options;
        }

        /// <summary>Adds the authentication.</summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        public async override Task AddAuthentication(HttpClient client, 
            HttpRequestMessage request)
        {
            if (string.IsNullOrEmpty(token.AccessToken)
                || DateTimeOffset.UtcNow - token.CreationDate >= TimeSpan.FromHours(7))
            {
                await RequestNewToken(client);
            }
            else if (DateTimeOffset.UtcNow - token.CreationDate >= TimeSpan.FromHours(2))
            {
                await RefreshToken(client);
            }

            request.Headers.Add("Authorization", $"Bearer {token.AccessToken}");

        }

        private async Task RequestNewToken(HttpClient client)
        {
            try
            {
                var response = await client.PostAsync($"{this.options.Address.ToBaseUrl()}/auth?type=auth",
                    new StringContent(Json.Serialize(new GetTokenRequest()
                    {
                        Login = options.Login,
                        Password = options.Password
                    }), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    var tokenResponse = Json.Deserialize<GetTokenResponse>(await response.Content.ReadAsStringAsync());
                    if (string.IsNullOrEmpty(tokenResponse.AccessToken)
                        || string.IsNullOrEmpty(tokenResponse.RefreshToken))
                    {
                        throw new ApiException(
                            $"Bad Autentication to the lndhub : {this.options.Address}",
                            HttpStatusCode.Unauthorized);
                    }

                    this.token.AccessToken = tokenResponse.AccessToken;
                    this.token.RefreshToken = tokenResponse.RefreshToken;
                    this.token.CreationDate = DateTimeOffset.UtcNow;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    throw new ApiException(
                        $"Bad Autentication to the lndhub : {this.options.Address} with error status code {response.StatusCode} and response {errorContent}",
                        HttpStatusCode.Unauthorized);
                }
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception exc)
            {
                throw new ApiException($"Bad Autentication",
                   HttpStatusCode.Unauthorized,
                   innerException: exc);
            }
        }

        private async Task RefreshToken(HttpClient client)
        {
            try
            {
                var response = await client.PostAsync($"{this.options.Address.ToBaseUrl()}/auth?type=refresh_token",
                    new StringContent(Json.Serialize(new RefreshTokenRequest()
                    {
                        ResfreshToken = this.token.RefreshToken
                    }), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    var tokenResponse = Json.Deserialize<GetTokenResponse>(await response.Content.ReadAsStringAsync());
                    if (string.IsNullOrEmpty(tokenResponse.AccessToken)
                        || string.IsNullOrEmpty(tokenResponse.RefreshToken))
                    {
                        throw new ApiException(
                            $"Cannot refresh token for {this.options.Address}",
                            HttpStatusCode.Unauthorized);
                    }

                    this.token.AccessToken = tokenResponse.AccessToken;
                    this.token.RefreshToken = tokenResponse.RefreshToken;
                    this.token.CreationDate = DateTimeOffset.UtcNow;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    throw new ApiException(
                        $"Cannot refresh token to the lndhub : {this.options.Address} with error status code {response.StatusCode} and response {errorContent}",
                        HttpStatusCode.Unauthorized);
                }
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception exc)
            {
                throw new ApiException($"Cannot refresh token",
                   HttpStatusCode.Unauthorized,
                   innerException: exc);
            }
        }
    }
}
