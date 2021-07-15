# C-Lightning Client

## Introduction

C-Lightning Client is shipped with `LightningPay` package.

[More info about C-Lightning](https://github.com/ElementsProject/lightning) (Github Project)

## Create the client

### Instantiate

```c#
using (var client = CLightningClient.New("tcp://127.0.0.1:9835"))
{
	//Your code...
}
```

### Sample

You can retrieve a code sample with LND Client here : [C-Lightning Client Sample](/samples/LightningPay.Samples.Console/CLightningClientSample.cs)

## Dependency Injection

`LightningPay.DependencyInjection` package adds extension method to create the C-Lightning Client with .NET Core Dependency Injection in your startup file : 

```c#
public void ConfigureServices(IServiceCollection services)
{
	///...
    
	services.AddCLightningClient(new Uri("tcp://127.0.0.1:9835"));
}
```

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

        services.AddCLightningClient(new Uri("tcp://127.0.0.1:9835"));
    }
```