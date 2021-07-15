using System;

using System.Threading.Tasks;

namespace LightningPay.Clients.CLightning
{
    /// <summary>
    ///   C-Lightning tcp client interface
    /// </summary>
    public interface ICLightningTcpClient
    {
        /// <summary>Sends the command asynchronous.</summary>
        /// <typeparam name="Response">The type of the esponse.</typeparam>
        /// <param name="address">The address.</param>
        /// <param name="command">The command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<Response> SendCommandAsync<Response>(Uri address,  string command, object[] parameters = null);
    }
}
