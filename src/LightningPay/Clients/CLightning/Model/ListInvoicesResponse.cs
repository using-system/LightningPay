using System.Collections.Generic;

namespace LightningPay.Clients.CLightning
{
    internal class ListInvoicesResponse
    {
        [Serializable("invoices")]
        public List<CLightningInvoice> Invoices { get; set; }
    }
}
