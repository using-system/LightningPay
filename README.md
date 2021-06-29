# LightningPay
Bitcoin Lightning Network Payment .NET library (.NET Standard 2.0)

## Features

- [x] Create an invoice ([Documentation](documentation/client.md))
- [x] Check invoice payment ([Documentation](documentation/client.md))
- [ ] Pay an invoice  (Not supported yet)
- [ ] Send money  (Not supported yet)

More features will be supported in futures versions. 

## Packages

- `LightningPay` : Core library with zero dependency
- `LightningPay.DependencyInjection` : Extension methods for .NET Core Dependency injection mechanism (`IServiceCollection`)

## Integration

Connect to your lightning nodes : 

- [x] LND ([Documentation](documentation/client-lnd.md))
- [ ] C-Lightning  (Not supported yet)
- [ ] Eclair  (Not supported yet)
- [ ] Charge (Not supported yet)

Or with custodial solution for lightning (without having a node  of your own) : 

- [x] LNDHub  ([Documentation](documentation/client-lndhub.md))
- [ ] LNbits  (Not supported yet)

## Samples

Need for code samples ? Go here : [Lightning samples](samples/)

## Documentation

[Lightning Documentation](documentation/)

## Continuous Integration

[LightningPay build reports](https://dev.azure.com/NiawaCorp/LightningPay/_build?definitionId=24)

## Contributing

We appreciate new contributions.

- Non developer : You found a bug or have an suggestion for a new feature ? Don't hesitate to create an issue
- Developer : develop branch is the principal branch for development. This branch is protected. You must create a pull request and target this branch for code validation.