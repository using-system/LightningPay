# LNDHub Client

## Introduction

LNDHub Client is shipped with `LightningPay` package (NET Standard 2.0 library).

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

The AddLndHubLightningClient method has optionnal pamameters to configure your client : 

| Property name         | Type     | Description                                                  |
| --------------------- | -------- | ------------------------------------------------------------ |
| Address               | `Uri`    | Address of your node server with port (example : http://localhost:42802/) |
| login                 | `String` | LNDHub login                                                 |
| password              | `String` | LNDHub Password                                              |
| certificateThumbprint | `String` | Certificate thumbprint used for your https address if the certificate is not public<br />Ex : "284800A04D0C046636EBE60C37A4F527B8B550F3" |
| allowInsecure         | `bool`   | If you use https address, determine if you allow non secure transport (certificateThumbprint parameter will be ignored) |

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