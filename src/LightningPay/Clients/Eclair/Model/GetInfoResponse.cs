namespace LightningPay.Clients.Eclair
{
    internal class GetInfoResponse
    {
        [Json("nodeId")]
        public string NodeId { get; set; }
    }
}
