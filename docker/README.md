## How to test

## Docker Compose

You can use our docker-compose.yml file to test the LightningPay clients with Lnd.

You just have to enter the command line : 

```
docker-compose up
```

## Configuration

One the docker-compose is up, you have access to : 

- LND : http://localhost:32736/ (No Macaroon authentication)
- C-Lightning : tcp://127.0.0.1:48532 
- Eclair :  http://localhost:4570/ (Password = "eclairpassword")

## Credits

Thanks to [BTCPayServer](https://github.com/btcpayserver) for the docker images and the original docker-compose.yml that you can find [here](https://github.com/btcpayserver/BTCPayServer.Lightning/blob/master/tests/docker-compose.yml).