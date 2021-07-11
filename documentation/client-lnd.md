# LND Client

## Introduction

LND Client is shipped with `LightningPay` package.

[More info about LND](https://github.com/lightningnetwork/lnd) (Github Project)

## Create the client

### Instantiate

```c#
string macaroon = null; // Enter here your macaroon
using (var client = LndClient.New("http://localhost:8080/", macaroon))
{
	//Your code...
}
```

If you wants to use your own HttpClient to request the Lnd API, you can send it with the parameter "httpClient" of the method New() : 

```c#
using (HttpClient httpClient = new HttpClient())
{
	var lndClient = LndClient.New("http://localhost:8080/", httpClient: httpClient);
    
	//Your code...
}
```

### Sample

You can retrieve a code sample with LND Client here : [LND Client Sample](/samples/LightningPay.Samples.Console/LndClientSample.cs)

## Dependency Injection

`LightningPay.DependencyInjection` package adds extension method to create the LND Client with .NET Core Dependency Injection in your startup file : 

```c#
public void ConfigureServices(IServiceCollection services)
{
	///...

	string yourMacaroon = null;
	services.AddLndLightningClient(new Uri("http://localhost:8080/"), yourMacaroon);
}


```

### Options

The `AddLndLightningClient` method has optionnal pamameters to configure your client : 

| Parameter name        | Type     | Required | Description                                                  |
| --------------------- | -------- | -------- | ------------------------------------------------------------ |
| address               | `Uri`    | Yes      | Address of your node server with port (example : http://localhost:8080/) |
| macaroonHexString     | `String` | No       | Authentication assertion in hex string format<br /><u>Tip :</u> To get the hex string of your, type the command xxd -p -c2000 admin.macaroon to get the hex representation of your file. |
| macaroonBytes         | `byte[]` | No       | Authentication assertion in Byte array (to load macaron from file with .NET code `File.ReadAllBytes(macaroonFilePath)` ) |
| certificateThumbprint | `String` | No       | Certificate thumbprint used for your https address if the certificate is not public<br />Ex : "284800A04D0C046636EBE60C37A4F527B8B550F3" |
| allowInsecure         | `bool`   | No       | If you use https address, determine if you allow non secure transport (certificateThumbprint parameter will be ignored) |

### Use to the LightningPay Client

Once you register `LightningPay`, you can use the [client](/documentation/client.md) in any object by dependency injection in constructor : 

```c#
private readonly ILightningClient lightningClient;

public HomeController(ILightningClient lightningClient)
{
        this.lightningClient = lightningClient;
}
```

### Sample

You can retrieve a code samples used Dependency Injection in the Visual Studio Solution [`WebApp.sln`](/samples)

```c#
public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();

        /*
         * 
         * ############## LND Samples ##################
         *  - With no macaroon authentication : 
         *      services.AddLndLightningClient(new Uri("http://localhost:8080/"));
         *  - With macaroon authentication (with hex string) : 
         *      services.AddLndLightningClient(new Uri("http://localhost:8080/"),
                    macaroonHexString: "020...72c8");
         * - With macaroon authentication (Load macaroon file) : 
         *       services.AddLndLightningClient(new Uri("http://localhost:8080/"),
                    macaroonBytes: File.ReadAllBytes("/root/.lnd/data/chain/bitcoin/mainnet/invoice.macaroon"));
         * 
         * 
         */

        services.AddLndLightningClient(new Uri("http://localhost:8080/"));
    }
```