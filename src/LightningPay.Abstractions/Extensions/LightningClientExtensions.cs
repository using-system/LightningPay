using System;

namespace LightningPay
{
    /// <summary>
    ///   Lightning clients extension methods
    /// </summary>
    public static class LightningClientExtensions
    {
        /// <summary>Converts to a REST client.</summary>
        /// <param name="client">The  lightning client.</param>
        /// <returns>
        ///  Lightning Rest client
        /// </returns>
        /// <exception cref="System.ArgumentException">Lighntning client is not a rest LBBits client !</exception>
        public static IRestLightningClient ToRestClient(this ILightningClient client)
        {
            var restClient = client as IRestLightningClient;

            if (restClient == null)
            {
                throw new ArgumentException("Lighntning client is not a rest client !");
            }

            return restClient;
        }

        /// <summary>Converts to a rpc client.</summary>
        /// <param name="client">The  lightning client.</param>
        /// <returns>
        ///  Lightning Rpc client
        /// </returns>
        /// <exception cref="System.ArgumentException">Lighntning client is not a rest LBBits client !</exception>
        public static IRpcLightningClient ToRpcClient(this ILightningClient client)
        {
            var rpcClient = client as IRpcLightningClient;

            if (rpcClient == null)
            {
                throw new ArgumentException("Lighntning client is not a rpc client !");
            }

            return rpcClient;
        }
    }
}
