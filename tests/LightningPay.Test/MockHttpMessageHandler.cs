using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LightningPay.Test
{
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly (string Response, HttpStatusCode StatusCode)[] responses;

        public List<HttpRequestMessage> Requests { get;  }

        public MockHttpMessageHandler(params (string Response, HttpStatusCode StatusCode)[] responses)
        {
            this.Requests = new List<HttpRequestMessage>();
            this.responses = responses;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            this.Requests.Add(request);

            return Task.FromResult(new HttpResponseMessage
            {
                StatusCode = responses[this.Requests.Count-1].StatusCode,
                Content = new StringContent(responses[this.Requests.Count - 1].Response)
            });
        }
    }
}
