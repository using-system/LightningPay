# LightningPay client

## Interface

Every clients of the `LightningPay` package implements the interface [`ILightningClient`](/src/LightningPay/ILightningClient.cs) : 

```c#
public interface ILightningClient : IDisposable
{
	Task<LightningInvoice> CreateInvoice(long satoshis, string description, CreateInvoiceOptions options = null);

	Task<bool> CheckPayment(string invoiceId);
}
```

## Create an invoice

### Standard use

To create an invoice, call the method `CreateInvoice` with the 2 parameters required : satoshis and the description will be appear on the invoice : 

```c#
var invoice = await lndClient.CreateInvoice(100, "My First Invoice");
```

### Optional parameters

You can add optional parameters with the CreateInvoiceOptions object : 

| Property | Type      | Description                                                  |
| -------- | --------- | ------------------------------------------------------------ |
| Expiry   | TimeSpan? | Duration before which the invoice will expire (Default value : 1 Day) |

```c#
var invoice = await lndClient.CreateInvoice(100 , "My First invoice", 
         new CreateInvoiceOptions(TimeSpan.FromHours(2)));
```



## Check invoice Payment

Once the invoice is created, you can check the payment of the invoice simply by call the `CheckPayment` method with the invoice identifier.

You can retrieve the invoice identifier with the `LightningInvoice` object returned by the `CreateInvoice` method : 

```c#
var invoice = await lndClient.CreateInvoice(100, "My First Invoice", TimeSpan.FromMinutes(5));

while (! await lndClient.CheckPayment(invoice.Id))
{
	System.Console.WriteLine("Waiting for invoice payment....");
	await Task.Delay(5000);
}
```

