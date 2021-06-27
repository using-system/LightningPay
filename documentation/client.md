# LightningPay client

## Interface

Every clients of the `LightningPay` package implements the interface [`ILightningClient`](/src/LightningPay/ILightningClient.cs) : 

```c#
public interface ILightningClient : IDisposable
{
	Task<LightningInvoice> CreateInvoice(long satoshis, string description, TimeSpan expiry);

	Task<bool> CheckPayment(string invoiceId);
}
```

## Create an invoice



## Check invoice Payment

Once the invoice is created, you can check the payment of the invoice simply by call the `CheckPayment` method with the invoice identifier.

You can retrieve the invoice identifier with the `LightningInvoice` object returned by the `CreateInvoice` method : 

```c#
var invoice = await lndClient.CreateInvoice(1, "Test", TimeSpan.FromMinutes(5));

while (! await lndClient.CheckPayment(invoice.Id))
{
	System.Console.WriteLine("Waiting for invoice payment....");
	await Task.Delay(5000);
}
```

