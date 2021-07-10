using System.Threading.Tasks;

namespace LightningPay
{
    /// <summary>
    ///   Lightning client that's use JSON RESTful API
    /// </summary>
    public interface IRestLightningClient : ILightningClient
    {
        /// <summary>Request the specified URL with GET verb.</summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="url">The URL to request.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<TResponse> Get<TResponse>(string url)
            where TResponse : class;

        /// <summary>Request the specified URL with POST verb.</summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="url">The URL to request.</param>
        /// <param name="body">The body to post.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<TResponse> Post<TResponse>(string url, object body = null)
            where TResponse : class;

        /// <summary>Request the specified URL with PUT verb.</summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="url">The URL to request.</param>
        /// <param name="body">The body to post.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<TResponse> Put<TResponse>(string url, object body = null)
            where TResponse : class;

        /// <summary>Request the specified URL with custom verb.</summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="httpMethod">Http method</param>
        /// <param name="url">The URL to request.</param>
        /// <param name="body">The body to post.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<TResponse> Request<TResponse>(string httpMethod, string url, object body = null)
            where TResponse : class;
    }
}
