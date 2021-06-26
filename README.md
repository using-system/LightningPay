# LightningPay
Bitcoin Lightning Network Payment  library (.NET Standard 2.0)

## Features

- Create an invoice
- Check invoice payment

More features will be supported in futures versions (payment, send money...). 

## Integration

Connect to your lightning nodes : 

- [x] LND ([Documentation](documentation/client-lnd.md))
- [ ] C-Lightning  (Not supported yet)
- [ ] Eclair  (Not supported yet)
- [ ] Charge (Not supported yet)

Or with custodial solution for lightning (without having a node  of your own) : 

- [x] LNDHub
- [ ] LNbits  (Not supported yet)

## Packages

- `LightningPay` : Core library with 0 dependency
- `LightningPay.DependencyInjection` : Extension methods for .NET Core Dependency injection mechanism (`IServiceCollection`)

## Samples

Need for code samples ? Go here : [Lightning samples](samples/)

## Documentation

[Lightning Documentation](documentation/)

## Contributing

We appreciate new contributions.

- Non developer : You found a bug or have an suggestion for a new feature ? Don't hesitate to create an issue
- Developer : develop branch is the principal branch for development. This branch is protected. You must create a pull request and target this branch for code validation.