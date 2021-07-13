using System;
using System.Threading.Tasks;

namespace LightningPay
{
    /// <summary>
    /// Every clients of the LightningPay package implements the interface ILightningClient
    /// </summary>
    public interface ILightningClient : IDisposable
    {
        /// <summary>Checks the connectivity.</summary>
        /// <returns>
        ///    True of the connectivity is ok, false otherwise 
        /// </returns>
        Task<CheckConnectivityResponse> CheckConnectivity();

        /// <summary>Gets the node / wallet balance.</summary>
        /// <returns>
        ///   Balance
        /// </returns>
        Task<Money> GetBalance();

        /// <summary>Creates the invoice.</summary>
        /// <param name="amount">The amount to receive.</param>
        /// <param name="description">The description will be appears in the invoice.</param>
        /// <param name="options">Invoice creation options.</param>
        /// <returns>
        ///   The lightning invoice just created
        /// </returns>
        Task<LightningInvoice> CreateInvoice(Money amount, string description, CreateInvoiceOptions options = null);

        /// <summary>Checks the payment of an invoice.</summary>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <returns>
        ///  True of the invoice is paid, false otherwise 
        /// </returns>
        Task<bool> CheckPayment(string invoiceId);

        /// <summary>Pay.</summary>
        /// <param name="paymentRequest">The payment request (aka bolt11).</param>
        /// <returns>
        ///   PaymentResponse
        /// </returns>
        Task<PaymentResponse> Pay(string paymentRequest);
    }
}
