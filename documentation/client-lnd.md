# LND Client

## Introduction

LND Client is shipped with LightningPay package (NET Standard 2.0 library).

## Create the client

### Instantiate

```c#
using System;
using System.Net.Http;

using LightningPay.Clients.Lnd;

namespace Samples
{
    class LndClientSample
    {
        public void CreateClient()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var lndClient = new LndClient(httpClient, new LndOptions()
                {
                    BaseUri = new Uri("http://localhost:42802/")
                });
            }
        }
    }
}
```

### Configuration

You can configure the LND client with the `LndOptions` class : 

| Property name | Type     | Description                                                  |
| ------------- | -------- | ------------------------------------------------------------ |
| BaseUri       | `Uri`    | Address of your node server with port (example : http://localhost:42802/) |
| Macaroon      | `byte[]` | Authentication assertion                                     |

### Sample

You can retrieve a code sample with LND Client here : [LND Client Sample](/samples/LightningPay.Samples.Console/LndClientSample.cs)