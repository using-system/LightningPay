# Extensions

## Introduction

With `LightningPay`, you can easly extend your client by add extension methods to the `ILightningClient` interface.

## Step 1

Add the reference to the nuget package `LightningPay.Abstractions`.

## Step 2

Create an extension method for `ILightningClient` interface

```c#
public static async Task<string> MyExtensionMethod(this ILightningClient client, 
            long param1,
            string param2)
     {
     	//...
     }
```

## Step 3

There are 2 type of client : Rest client (`IRestLightningClient`) and Rpc (`IRpcLightningClient`) client. Below is the method to call to retrieve the corresponding client : 

| Client      | Type                 | Method to call |
| ----------- | -------------------- | -------------- |
| LND         | IRestLightningClient | ToRestClient() |
| C-Lightning | IRpcLightningClient  | ToRpcClient()  |
| Eclair      | IRestLightningClient | ToRestClient() |
| LNBits      | IRestLightningClient | ToRestClient() |
| LNDHub      | IRestLightningClient | ToRestClient() |

For a rest client : 

```c#
var restClient = client.ToRestClient();
```

For a rcp client : 

```c#
var rpcClient = client.ToRpcClient();
```

## Step 4

Once your get your rest client or rcp client, just call the methods shipped with the interface.

Rest client (`IRestLightningClient`) : 

```c#
public interface IRestLightningClient : ILightningClient
{
        Task<TResponse> Get<TResponse>(string url)
            where TResponse : class;

        Task<TResponse> Post<TResponse>(string url, object body = null, bool formUrlEncoded = false)
        	where TResponse : class;

        Task<TResponse> Request<TResponse>(string httpMethod, string url, object body = null, bool formUrlEncoded = false)
        	where TResponse : class;
}
```

Rpc client (`IRpcLightningClient`) : 

```c#
public interface IRpcLightningClient : ILightningClient
{
        IRpcClient Client { get; }
}
public interface IRpcClient
{
    Task<Response> SendCommandAsync<Response>(string command, params object[] parameters);
}
```

For your model (Response and body / parameters), you can use the attribute Serializable to define the name of the api property name : 

```c#
internal class CLightningInvoice
{

    [Serializable("payment_hash")]
    public string PaymentHash { get; set; }

}
```

## Sample

You can retrieve a code samples in the Visual Studio Solution [`LNBitsExtensions.sln`](/samples) for the LNBits client.

```c#
public static async Task<string> AddLNUrlp(this ILightningClient client, 
            long amount,
            string description)
{
            var response = await client.ToRestClient()
                .Post<CreatePayLinkResponse>("lnurlp/api/v1/links", new CreatePayLinkRequest()
            {
                Amount = amount,
                Min = amount,
                Max = amount,
                Description = description,
                MaxCommentChars = 20
            });

         return response.Url;
}

internal class CreatePayLinkRequest
{
        [Serializable("description")]
        public string Description { get; set; }

        [Serializable("amount")]
        public long Amount { get; set; }


        [Serializable("min")]
        public long Min { get; set; }


        [Serializable("max")]
        public long Max { get; set; }

        [Serializable("comment_chars")]
        public int MaxCommentChars { get; set; }

}

internal class CreatePayLinkResponse
{
        [Serializable("lnurl")]
        public string Url { get; set; }
}
```

