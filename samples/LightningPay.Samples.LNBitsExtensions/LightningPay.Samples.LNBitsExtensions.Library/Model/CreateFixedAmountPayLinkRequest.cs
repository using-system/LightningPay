namespace LightningPay.Samples.LNBitsExtensions.Library
{
    internal class CreateFixedAmountPayLinkRequest
    {
        [Json("description")]
        public string Description { get; set; }

        [Json("amount")]
        public long Amount { get; set; }
    }
}
