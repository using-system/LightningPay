using System.Collections.Generic;

namespace LightningPay.Clients.CLightning
{
    internal class ListInvoicesResponse
    {
        public ListInvoicesResponse()
        {
            this.Invoices = new List<CLightningInvoice>();
        }

        [Serializable("invoices")]
        public List<CLightningInvoice> Invoices { get; set; }
    }
}
