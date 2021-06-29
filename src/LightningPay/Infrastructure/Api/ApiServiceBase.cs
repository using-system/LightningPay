using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;

using LightningPay.Tools;

namespace LightningPay.Infrastructure.Api
{
    /// <summary>
    ///   Client api base class
    /// </summary>
    public abstract class ApiServiceBase
    {
        protected readonly HttpClient httpClient;

        private readonly AuthenticationBase authentication;

        /// <summary>Initializes a new instance of the <see cref="ApiServiceBase" /> class.</summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="authentication">The authentication.</param>
        public ApiServiceBase(HttpClient httpClient, 
            AuthenticationBase authentication)
        {
            this.httpClient = httpClient;
            this.authentication = authentication;
        }

        /// <summary>Sends the asynchronous.</summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="method">The method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="body">The body.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <exception cref="LightningPay.ApiException">Http error with status code {response.StatusCode} and response {errorContent}
        /// or
        /// Internal Error on request the url : {url} : {exc.Message}</exception>
        protected async Task<TResponse> SendAsync<TResponse>(HttpMethod method,
           string url,
           object body = null)
           where TResponse : class
        {
            HttpContent content = null;

            if (body != null)
            {
                content = new StringContent(Json.Serialize(body), Encoding.UTF8, "application/json");
            }

            var request = new HttpRequestMessage(method, url)
            {
                Content = content
            };
            await authentication.AddAuthentication(this.httpClient, request);

            try
            {
                var response = await this.httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(json))
                    {
                        var responseObject = Json.Deserialize<TResponse>(json, new JsonOptions() 
                        {
                            SerializationOptions = JsonSerializationOptions.ByteArrayAsBase64
                        }
                        );

                        return responseObject;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    throw new ApiException($"Http error with status code {response.StatusCode} and response {errorContent}",
                        response.StatusCode,
                        responseData: errorContent);
                }

            }
            catch(ApiException)
            {
                throw;
            }
            catch (Exception exc)
            {
                throw new ApiException($"Internal Error on request the url : {url} : {exc.Message}", 
                    HttpStatusCode.InternalServerError, 
                    innerException: exc);
            }

        }

    }
}
