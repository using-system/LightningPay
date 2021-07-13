# LightningPay
Bitcoin Lightning Network Payment .NET library

## Features

- [x] Get balance ([Documentation](documentation/client.md#get-wallet-balance))
- [x] Create an invoice ([Documentation](documentation/client.md#create-an-invoice))
- [x] Check payment ([Documentation](documentation/client.md#check-invoice-payment))
- [x] Pay an invoice ([Documentation](documentation/client.md#pay))

More features will be supported in futures versions. 

## Packages

- `LightningPay.Abstractions` [![NuGet](https://img.shields.io/nuget/v/LightningPay.Abstractions.svg)](https://www.nuget.org/packages/LightningPay.Abstractions) : Interfaces and model used by `LightningPay` 
- `LightningPay` [![NuGet](https://img.shields.io/nuget/v/LightningPay.svg)](https://www.nuget.org/packages/LightningPay) : Core library with all Lightning clients
- `LightningPay.DependencyInjection` [![NuGet](https://img.shields.io/nuget/v/LightningPay.DependencyInjection.svg)](https://www.nuget.org/packages/LightningPay.DependencyInjection) : Extension methods for .NET DI

See [Which package to use ?](documentation/packages.md) for more details.

## Integration

Connect to your lightning nodes : 

- [x] LND ([Documentation](documentation/client-lnd.md))
- [x] Eclair  ([Documentation](documentation/client-eclair.md))
- [ ] C-Lightning  (Not supported yet)
- [ ] Charge (Not supported yet)

Or with custodial solution for lightning (without having a node  of your own) : 

- [x] LNDHub / BlueWallet ([Documentation](documentation/client-lndhub.md))
- [x] LNBits ([Documentation](documentation/client-lnbits.md))

## Samples

Need for code samples ? Go here : [Lightning samples](samples/)

## Documentation

[Lightning Documentation](documentation/)

## Local test

You can use our [docker-compose file](docker/) to test the `LightningPay` clients.

## Continuous Integration

[LightningPay build reports](https://dev.azure.com/NiawaCorp/LightningPay/_build?definitionId=24)

## Contributing

We appreciate new contributions.

- Non developer : You found a bug or have an suggestion for a new feature ? Don't hesitate to create an issue
- Developer : develop branch is the principal branch for development. This branch is protected. You must create a pull request and target this branch for code validation.