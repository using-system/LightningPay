namespace LightningPay.Samples.LNBitsExtensions.Library
{
    internal class CreatePayLinkRequest
    {
        [Json("description")]
        public string Description { get; set; }

        [Json("amount")]
        public long Amount { get; set; }


        [Json("min")]
        public long Min { get; set; }


        [Json("max")]
        public long Max { get; set; }

        [Json("comment_chars")]
        public int MaxCommentChars { get; set; }

    }
}
