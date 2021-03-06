# LightningPay client

## Interface

Every clients of the `LightningPay` package implements the interface [`ILightningClient`](/src/LightningPay.Abstractions/ILightningClient.cs) : 

```c#
public interface ILightningClient : IDisposable
{
	Task<CheckConnectivityResponse> CheckConnectivity();
    
	Task<Money> GetBalance();
    
	Task<LightningInvoice> CreateInvoice(Money amount, string description, CreateInvoiceOptions options = null);

	Task<bool> CheckPayment(string invoiceId);
    
	Task<PaymentResponse> Pay(string paymentRequest);
}
```

## Check connectivity

To check the connectivity of your node / wallet, call the method CheckConnectivity : 

```c#
var response = await client.CheckConnectivity();
Console.WriteLine($"Check connectivity  result : {response.Result}");
```

Response object contains the result (an enumeration) and the error message if the connectivity is KO :

```c#
public class CheckConnectivityResponse
{
	public CheckConnectivityResult Result { get; set; }

	public string Error { get; set; }
}

public enum CheckConnectivityResult
{
	Ok,
	Error
}
```



## Get wallet balance

To get the wallet balance (amount in satoshis), call the method `GetBalance` : 

```c#
var balance = await client.GetBalance();
Console.WriteLine($"Wallet balance : {balance.ToSatoshis()} sat");
```

## Create an invoice

### Standard use

To create an invoice, call the method `CreateInvoice` with the 2 parameters required : satoshis and the description will be appear on the invoice : 

```c#
var invoice = await client.CreateInvoice(Money.FromSatoshis(100), "My First Invoice");
```

### Optional parameters

You can add optional parameters with the CreateInvoiceOptions object : 

| Property | Type      | Description                                                  |
| -------- | --------- | ------------------------------------------------------------ |
| Expiry   | TimeSpan? | Duration before which the invoice will expire (Default value : 1 Day) |

```c#
var invoice = await client.CreateInvoice(Money.FromSatoshis(100) , "My First invoice", 
         new CreateInvoiceOptions(TimeSpan.FromHours(2)));
```

## Check invoice payment

Once the invoice is created, you can check the payment of the invoice simply by call the `CheckPayment` method with the invoice identifier.

You can retrieve the invoice identifier with the `LightningInvoice` object returned by the `CreateInvoice` method : 

```c#
var invoice = await client.CreateInvoice(Money.FromSatoshis(100), "My First Invoice");

while (! await client.CheckPayment(invoice.Id))
{
	Console.WriteLine("Waiting for invoice payment....");
	await Task.Delay(5000);
}
```

## Pay

To pay an invoice with a payment request (BOLT11), call the method `Pay` : 

```c#
var response = await client.Pay("lnbc1....d84c");
Console.WriteLine($"Payment result : {response.Result}");
```

Response object contains the result of the payment (an enumeration) and the error message if the payment failed :

```c#
public class PaymentResponse
{
	public PayResult Result { get; set; }

	public string Error { get; set; }
}

public enum PayResult
{
	Ok,
	Error
}
```