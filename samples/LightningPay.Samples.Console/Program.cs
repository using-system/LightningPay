﻿using System.Threading.Tasks;

namespace LightningPay.Samples.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            SampleBase sample = new LndHubClientSample();

            await sample.Execute();

            System.Console.WriteLine("Ended !");
            System.Console.ReadLine();
        }
    }
}
