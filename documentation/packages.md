# Which package to use ?

## LightningPay

`LightningPay` package contains all Lightning cliens (LND, LNDHub, LNBits...).

It's the ideal starting point for .NET client application (console, wpf, mobile) that not use Dependency Injection or if you want to instantiate the client yourself.

## LightningPay.DependencyInjection

Use directly `LightningPay.DependencyInjection` for all your app projects that use Dependency Injection (Web API, Web App, Worker Service...)

## LightningPay.Abstractions

`LightningPay.Abstractions` is for your all library (non app projects) to avoid to add strong reference to the lightning clients.

