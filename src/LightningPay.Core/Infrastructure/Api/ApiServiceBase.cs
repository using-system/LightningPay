using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;

using Newtonsoft.Json;

namespace LightningPay.Core.Infrastructure.Api
{
    public abstract class ApiServiceBase : IDisposable
    {
        protected readonly HttpClient httpClient;

        public ApiServiceBase(HttpClient httpClient)
        {
            this.httpClient = httpClient;
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
                content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            }

            var request = new HttpRequestMessage(method, url)
            {
                Content = content
            };

            try
            {
                var response = await this.httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(json))
                    {
                        var responseObject = JsonConvert.DeserializeObject<TResponse>(json);

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

        public virtual void Dispose()
        {
            this?.httpClient?.Dispose();
        }
    }
}
