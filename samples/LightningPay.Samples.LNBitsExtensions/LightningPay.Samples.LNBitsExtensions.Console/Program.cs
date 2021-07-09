using System.Threading.Tasks;

using LightningPay.Clients.LNBits;
using LightningPay.Samples.LNBitsExtensions.Library;

namespace LightningPay.Samples.LNBitsExtensions.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ILightningClient client = LNBitsClient.New(apiKey: "YourApiKey");

            string url = await client.AddLNUrlp(500, "My Pay link");

            System.Console.WriteLine($"Pay link created : {url}");
            System.Console.ReadLine();
        }
    }
}
