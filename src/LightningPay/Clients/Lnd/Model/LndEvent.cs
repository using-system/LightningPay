namespace LightningPay.Clients.Lnd
{
    internal class LndEvent<TEvent>
    {
        [Serializable("result")]
        public TEvent Result { get; set; }

        [Serializable("error")]
        public string Error { get; set; }
    }
}
