using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;

using LightningPay.Tools;

namespace LightningPay.Infrastructure.Api
{
    public abstract class ApiServiceBase
    {
        protected readonly HttpClient httpClient;

        private readonly AuthenticationBase authentication;

        public ApiServiceBase(HttpClient httpClient, 
            AuthenticationBase authentication)
        {
            this.httpClient = httpClient;
            this.authentication = authentication;
        }

        protected async Task<string> GetStringAsync(string url)
        {
            return await this.httpClient.GetStringAsync(url);
        }

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
            authentication.AddAuthentication(request);

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
                    var json = await response.Content.ReadAsStringAsync();

                    throw new ApiException("Internal error", json);
                }

            }
            catch (Exception)
            {
                throw new ApiException("Internal Error", "-1");
            }

        }

    }
}
