# Eclair Client

## Introduction

Eclair Client is shipped with `LightningPay` package.

[More info about Eclair](https://github.com/ACINQ/eclair) (Github Project)

## Create the client

### Instantiate

```c#
string password = "yourPassword"; // Enter here your macaroon
using (var client = EclairClient.New("http://localhost:8080/", password))
{
	//Your code...
}
```

If you wants to use your own HttpClient to request the Eclair API, you can send it with the parameter "httpClient" of the method New() : 

```c#
using (HttpClient httpClient = new HttpClient())
{
	var eclairClient = EclairClient.New("http://localhost:8080/", password, httpClient: httpClient);
    
	//Your code...
}
```

### Sample

You can retrieve a code sample with Eclair Client here : [Eclair Client Sample](/samples/LightningPay.Samples.Console/EclairClientSample.cs)

## Dependency Injection

`LightningPay.DependencyInjection` package adds extension method to create the Eclair Client with .NET Core Dependency Injection in your startup file : 

```c#
public void ConfigureServices(IServiceCollection services)
{
	///...

	string password = "yourPassword"; 
	services.AddEclairLightningClient(new Uri("http://localhost:8080/"), password);
}


```

### Options

The `AddEclairLightningClient` method has optionnal pamameters to configure your client : 

| Parameter name        | Type     | Required | Description                                                  |
| --------------------- | -------- | -------- | ------------------------------------------------------------ |
| address               | `Uri`    | Yes      | Address of your node server with port (example : http://localhost:8080/) |
| password              | `String` | No       | Eclair api password                                          |
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

	services.AddEclairLightningClient(new Uri("http://localhost:8080/"), 
		password: "YourPassword");
}
```