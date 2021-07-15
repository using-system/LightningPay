using System.Threading.Tasks;

namespace LightningPay
{
    /// <summary>
    ///   Rpc Lightning client interface
    /// </summary>
    public interface IRpcLightningClient : ILightningClient
    {
        /// <summary>Gets the rpc client.</summary>
        /// <value>The rpc client.</value>
        IRpcClient Client { get; }
    }

    /// <summary>
    ///  Rpc client interface
    /// </summary>
    public interface IRpcClient
    {
        /// <summary>Sends the command asynchronous.</summary>
        /// <typeparam name="Response">The type of the response.</typeparam>
        /// <param name="command">The command to execute.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        ///   Response
        /// </returns>
        Task<Response> SendCommandAsync<Response>(string command, params object[] parameters);
    }
}
