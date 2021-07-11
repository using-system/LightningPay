## How to test

## Docker Compose

You can use our docker-compose.yml file to test the LightningPay clients with Lnd.

You just have to enter the command line : 

```
docker-compose up
```

## Configuration

One the docker-compose is up, you have access to : 

- LND api : http://localhost:32736/ (No Macaroon authentication)

## Credits

Thanks to [BTCPayServer](https://github.com/btcpayserver) for the docker images and the original docker-compose.yml that you can find [here](https://github.com/btcpayserver/BTCPayServer.Lightning/blob/master/tests/docker-compose.yml).