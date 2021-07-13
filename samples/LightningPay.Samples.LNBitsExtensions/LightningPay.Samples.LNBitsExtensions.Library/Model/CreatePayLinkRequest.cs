namespace LightningPay.Samples.LNBitsExtensions.Library
{
    internal class CreatePayLinkRequest
    {
        [Serializable("description")]
        public string Description { get; set; }

        [Serializable("amount")]
        public long Amount { get; set; }


        [Serializable("min")]
        public long Min { get; set; }


        [Serializable("max")]
        public long Max { get; set; }

        [Serializable("comment_chars")]
        public int MaxCommentChars { get; set; }

    }
}
