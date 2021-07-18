using System.Collections.Generic;

namespace LightningPay.Clients.CLightning
{
    internal class ListInvoicesResponse
    {
        internal ListInvoicesResponse()
        {
            this.Invoices = new List<CLightningInvoice>();
        }

        [Serializable("invoices")]
        public List<CLightningInvoice> Invoices { get; set; }
    }
}
