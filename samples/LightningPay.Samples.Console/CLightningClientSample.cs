using System;
using System.Threading.Tasks;

using LightningPay.Clients.CLightning;


namespace LightningPay.Samples.Console
{
    class CLightningClientSample : SampleBase
    {
        public async override Task Execute()
        {
            using (CLightningClient client = CLightningClient.New("tcp://127.0.0.1:48532"))
            {
                System.Console.WriteLine($"check connectivity : {(await client.CheckConnectivity()).Result}");
            }
        }
    }
}
