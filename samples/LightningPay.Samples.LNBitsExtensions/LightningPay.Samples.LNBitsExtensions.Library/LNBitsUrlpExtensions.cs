using System.Threading.Tasks;

namespace LightningPay.Samples.LNBitsExtensions.Library
{
    public static class LNBitsUrlpExtensions
    {
        public static async Task<string> AddLNUrlp(this ILightningClient client, 
            long amount,
            string description)
        {
            var response = await client.ToRestClient()
                .Post<CreatePayLinkResponse>("lnurlp/api/v1/links", new CreatePayLinkRequest()
            {
                Amount = amount,
                Min = amount,
                Max = amount,
                Description = description,
                MaxCommentChars = 20
            });

            return response.Url;
        }
    
    }
}
