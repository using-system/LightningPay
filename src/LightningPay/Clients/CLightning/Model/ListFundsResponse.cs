using System.Collections.Generic;

namespace LightningPay.Clients.CLightning
{
    internal class ListFundsResponse
    {
        public ListFundsResponse()
        {
            this.Outputs = new List<Output>();
        }

        public List<Output> Outputs { get; set; }

        internal class Output
        {
            [Serializable("value ")]
            public long Value { get; set; }

            [Serializable("status")]
            public string Status { get; set; }
        }
    }
}
