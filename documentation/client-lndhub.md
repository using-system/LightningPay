# LNDHub Client

## Introduction

LNDHub (BlueWallet) Client is shipped with `LightningPay` package.

[More info about LNDHUB](https://github.com/BlueWallet/LndHub) (Github Project)

## Create the client

### Instantiate

```c#
string login = "MyLogin";
string password = "MyPassword";
using (var client = LndHubClient.New("https://lndhub.herokuapp.com/", login, password))
{
	//Your code...
}
```

If you wants to use your own HttpClient to request the LndHub API, you can send it with the parameter "httpClient" of the method New() : 

```c#
using (HttpClient httpClient = new HttpClient())
{
	var client = LndHubClient.New("https://lndhub.herokuapp.com/",login, password, httpClient: httpClient);
    
	//Your code...
}
```

### Sample

You can retrieve a code sample with LNDHub Client here : [LNDHub Client Sample](/samples/LightningPay.Samples.Console/LndHubClientSample.cs)

## Dependency Injection

`LightningPay.DependencyInjection` package adds extension method to create the LNDHub Client with .NET Core Dependency Injection in your startup file : 

```c#
public void ConfigureServices(IServiceCollection services)
{
	///...

	string login = "MyLogin";
	string password = "MyPassword";
	services.AddLndHubLightningClient(new Uri("https://lndhub.herokuapp.com/"),
		login, password);
}


```

### Options

The `AddLndHubLightningClient` method has optionnal pamameters to configure your client : 

| Parameter name        | Type     | Required | Description                                                  |
| --------------------- | -------- | -------- | ------------------------------------------------------------ |
| address               | `Uri`    | Yes      | Address of the LNDHub api (example : https://lndhub.herokuapp.com/) |
| login                 | `String` | Yes      | LNDHub login                                                 |
| password              | `String` | Yes      | LNDHub Password                                              |
| retryOnHttpError      | `int`    | No       | Number of retry on http error                                |
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
	//...
	services.AddLndHubLightningClient(new Uri("https://lndhub.herokuapp.com/"),
                    login: "2073282b41bad2955b74",
                    password: "a1c5a8c30a74bc3e8cbf"); // Puts yours credentials in config :)
}
```

