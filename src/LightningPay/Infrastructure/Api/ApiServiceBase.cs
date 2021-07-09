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
        private readonly string baseUrl;

        protected readonly HttpClient httpClient;

        private readonly AuthenticationBase authentication;

        /// <summary>Initializes a new instance of the <see cref="ApiServiceBase" /> class.</summary>
        /// <param name="baseurl">Base url of the api</param>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="authentication">The authentication.</param>
        public ApiServiceBase(string baseurl, 
            HttpClient httpClient, 
            AuthenticationBase authentication)
        {
            this.baseUrl = baseurl;
            this.httpClient = httpClient;
            this.authentication = authentication;
        }

        /// <summary>Request the specified URL with GET verb.</summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="url">The URL to request.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<TResponse> Get<TResponse>(string url)
           where TResponse : class
        {
            return await this.Send<TResponse>(HttpMethod.Get, url);
        }

        /// <summary>Request the specified URL with POST verb.</summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="url">The URL to request.</param>
        /// <param name="body">The body to post.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<TResponse> Post<TResponse>(string url, object body = null)
           where TResponse : class
        {
            return await this.Send<TResponse>(HttpMethod.Post, url, body);
        }

        /// <summary>Send web request.</summary>
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
        protected async Task<TResponse> Send<TResponse>(HttpMethod method,
           string url,
           object body = null)
           where TResponse : class
        {
            if(!url.StartsWith("http")
                && !string.IsNullOrEmpty(this.baseUrl))
            {
                url = $"{this.baseUrl}/{url.TrimStart('/')}";
            }

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
