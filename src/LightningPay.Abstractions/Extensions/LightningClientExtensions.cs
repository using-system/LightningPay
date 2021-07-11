using System;

namespace LightningPay
{
    /// <summary>
    ///   Lightning clients extension methods
    /// </summary>
    public static class LightningClientExtensions
    {
        /// <summary>Converts to restclient.</summary>
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
                throw new ArgumentException("Lighntning client is not a rest LBBits client !");
            }

            return restClient;
        }
    }
}
