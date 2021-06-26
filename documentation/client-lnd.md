# LND Client

## Introduction

LND Client is shipped with `LightningPay` package (NET Standard 2.0 library).

## Create the client

### Instantiate

```c#
using LightningPay.Clients.Lnd;

namespace Samples
{
    class LndClientSample
    {
        public void CreateClient()
        {
            byte[] macaroon = null; // Enter here your macaroon
            using (var client = LndClient.New("http://localhost:42802/", macaroon))
            {

            }
        }
    }

}
```

If you wants to use your own HttpClient to request the Lnd API, you can send it with the parameter "httpClient" of the method New() : 

```c#
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
                var lndClient = LndClient.New("http://localhost:42802/", httpClient: httpClient);
            }
        }
    }
}
```

### Sample

You can retrieve a code sample with LND Client here : [LND Client Sample](/samples/LightningPay.Samples.Console/LndClientSample.cs)

## Dependency Injection

LightningPay.DependencyInjection package adds extension method to create the LND Client with .NET Core Dependency Injection in your startup file : 

```c#
public void ConfigureServices(IServiceCollection services)
{
	///...

	byte[] yourMacaroon = null;
	services.AddLndLightningClient(new Uri("http://localhost:42802/"),
		macaroon: yourMacaroon);
}


```



### Options

You can configure the LND client with the `LndOptions` class : 

| Property name | Type     | Description                                                  |
| ------------- | -------- | ------------------------------------------------------------ |
| Address       | `Uri`    | Address of your node server with port (example : http://localhost:42802/) |
| Macaroon      | `byte[]` | Authentication assertion                                     |

