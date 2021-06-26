# LND Client

## Introduction

LND Client is shipped with LightningPay package (NET Standard 2.0 library).

## Create the client

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

