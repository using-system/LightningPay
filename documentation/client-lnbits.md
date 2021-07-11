# LNBits Client

## Introduction

LNBits Client is shipped with `LightningPay` package.

More info : 

- [Official website](https://lnbits.com/)
- [Github project](https://github.com/lnbits/lnbits)

## Create the client

### Instantiate

```c#
string apiKey = "YourApiKey";
using (var client = LNBitsClient.New("https://lnbits.com/", apiKey))
{
	//Your code...
}
```

If you wants to use your own HttpClient to request the LNBits API, you can send it with the parameter "httpClient" of the method New() : 

```c#
using (HttpClient httpClient = new HttpClient())
{
	var client = LNBitsClient.New("https://lnbits.com/", apiKey, httpClient: httpClient);
    
	//Your code...
}
```

### Sample

You can retrieve a code sample with LNBits Client here : [LNBits Client Sample](/samples/LightningPay.Samples.Console/LNBitsClientSample.cs)

## Dependency Injection

`LightningPay.DependencyInjection` package adds extension method to create the LNBits Client with .NET Core Dependency Injection in your startup file : 

```c#
public void ConfigureServices(IServiceCollection services)
{
	///...

	string apiKey = "YourApiKey";
	services.AddLNBitsLightningClient(new Uri("https://lnbits.com/"), apiKey);
}
```

### Options

The `AddLNBitsLightningClient` method has optionnal pamameters to configure your client : 

| Parameter name        | Type     | Required | Description                                                  |
| --------------------- | -------- | -------- | ------------------------------------------------------------ |
| address               | `Uri`    | Yes      | Address of the LNBits api (example : https://lnbits.com/)    |
| apiKey                | `String` | Yes      | LNBits api key                                               |
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

## LNBits extensions

### Build your extension methods

LNBits has a lot of extensions ! You can easly add your own method to implements the extension of your choice by add extension methods to the `ILightningClient` interface.

You just need the `LightningPay.Abstractions` package to create your extension methods.

### Sample

You can retrieve a code samples in the Visual Studio Solution  [`LNBitsExtensions.sln`](/samples)

```c#
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
```

