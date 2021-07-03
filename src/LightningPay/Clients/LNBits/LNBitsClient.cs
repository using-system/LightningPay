﻿using System;
using System.Threading.Tasks;
using System.Net.Http;

using LightningPay.Infrastructure.Api;

namespace LightningPay.Clients.LNBits
{
    /// <summary>
    ///   LNBits client
    /// </summary>
    public class LNBitsClient : ApiServiceBase, ILightningClient
    {
        private readonly string address;

        private bool clientInternalBuilt = false;

        /// <summary>Initializes a new instance of the <see cref="LNBitsClient" /> class.</summary>
        /// <param name="client">The client.</param>
        /// <param name="options">The options.</param>
        public LNBitsClient(HttpClient client,
            LNBitsOptions options) : base(client, BuildAuthentication(options))
        {
            this.address = options.Address.ToBaseUrl();
        }

        public Task<long> GetBalance()
        {
            throw new System.NotImplementedException();
        }

        public Task<LightningInvoice> CreateInvoice(long satoshis, string description, CreateInvoiceOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> CheckPayment(string invoiceId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Pay(string paymentRequest)
        {
            throw new System.NotImplementedException();
        }

        internal static AuthenticationBase BuildAuthentication(LNBitsOptions options)
        {
            if(string.IsNullOrEmpty(options?.ApiKey))
            {
                throw new ArgumentException("Api key required for LNBits authentication");
            }

            return new LNBitsAuthentication(options.ApiKey);
        }

        /// <summary>>Instanciate a new LNBits api.</summary>
        /// <param name="address">The address.</param>
        /// <param name="apiKey">The API key.</param>
        /// <param name="httpClient">The HTTP client.</param>
        /// <returns>
        ///   Return the LNBits client
        /// </returns>
        public static LNBitsClient New(string address = "https://lnbits.com/api/",
            string apiKey = "",
            HttpClient httpClient = null)
        {
            bool clientInternalBuilt = false;

            if (httpClient == null)
            {
                httpClient = new HttpClient();
                clientInternalBuilt = true;
            }

            LNBitsClient client = new LNBitsClient(httpClient, new LNBitsOptions()
            {
                Address = new Uri(address),
                ApiKey = apiKey
            });

            client.clientInternalBuilt = clientInternalBuilt;

            return client;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            if (this.clientInternalBuilt)
            {
                this.httpClient?.Dispose();
            }
        }
    }
}
