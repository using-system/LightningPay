using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using LightningPay.Tools;

namespace LightningPay.Clients.Eclair
{
    /// <summary>
    ///   Eclair events listener
    /// </summary>
    public class EclairListener : ILightningListener
    {
        private readonly string baseUrl;

        private readonly IEventSubscriptionsManager eventSubscriptionsManager;

        private readonly IServiceProvider serviceProvider;

        private ClientWebSocket socketClient;

        private CancellationTokenSource cts = new CancellationTokenSource();

        private readonly ArraySegment<byte> originalBuffer;

        private Task listenTask;

        /// <summary>Initializes a new instance of the <see cref="EclairListener" /> class.</summary>
        /// <param name="eventSubscriptionsManager">The event subscriptions manager.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="options">The options.</param>
        public EclairListener(IEventSubscriptionsManager eventSubscriptionsManager,
             IServiceProvider serviceProvider,
             EclairOptions options)
        {
            this.baseUrl = options.Address.ToBaseUrl();
            this.eventSubscriptionsManager = eventSubscriptionsManager;
            this.serviceProvider = serviceProvider;
            var bufferArray = new byte[1024 * 5];
            this.originalBuffer = new ArraySegment<byte>(bufferArray, 0, bufferArray.Length);
        }

        /// <summary>Subscribes to an event.</summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        public void Subscribe<TEvent, THandler>()
            where TEvent : LightningEvent
            where THandler : ILightningEventHandler<TEvent>
        {
            this.eventSubscriptionsManager.AddSubscription<TEvent, THandler>();
        }

        /// <summary>Unsubscribes to an event.</summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        public void Unsubscribe<TEvent, THandler>()
            where TEvent : LightningEvent
            where THandler : ILightningEventHandler<TEvent>
        {
            this.eventSubscriptionsManager.RemoveEventSubscription<TEvent, THandler>();
        }

        /// <summary>Starts listening the events.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task StartListening()
        {
            if(this.socketClient == null)
            {
                this.socketClient = new ClientWebSocket();
                await this.socketClient.ConnectAsync(new Uri($"{this.baseUrl}/ws"), this.cts.Token);
                this.listenTask = this.ListenLoop();
            }
        }


        private async Task ListenLoop()
        {
            while (!this.cts.IsCancellationRequested)
            {
                var message = await this.WaitMessage();
                if(string.IsNullOrEmpty(message))
                {
                    continue;
                }

                var @event = Json.Deserialize<EclairEvent>(message, new JsonOptions()
                {
                    SerializationOptions = JsonSerializationOptions.ByteArrayAsBase64
                });

                switch(@event.Type)
                {
                    case "payment-sent":
                        foreach (var handlerType in this.eventSubscriptionsManager.GetHandlersForEvent<PaymentSentEvent>())
                        {
                            //this.serviceProvider.CallEventHandler(handlerType, lightningEvent);
                        }
                        break;
                }
            }
        }

        private async Task<string> WaitMessage()
        {
            var buffer = this.originalBuffer;
            var array = this.originalBuffer.Array;
            var originalSize = this.originalBuffer.Array.Length;
            var newSize = this.originalBuffer.Array.Length;

            try
            {
                while (true)
                {
                    var message = await this.socketClient.ReceiveAsync(originalBuffer, this.cts.Token);
                    if (message.MessageType == WebSocketMessageType.Close)
                    {
                        await this.CloseSocket();
                        throw new LightningPayException("The socket has been closed", LightningPayException.ErrorCode.INTERNAL_ERROR);
                    }
                    if (message.MessageType != WebSocketMessageType.Text)
                    {
                        await this.CloseSocket();
                        throw new LightningPayException("The socket has been closed", LightningPayException.ErrorCode.INTERNAL_ERROR);
                    }

                    if (message.EndOfMessage)
                    {
                        buffer = new ArraySegment<byte>(array, 0, buffer.Offset + message.Count);
                        try
                        {
                            var o = new UTF8Encoding(false, true).GetString(buffer.Array, 0, buffer.Count);
                            if (newSize != originalSize)
                            {
                                Array.Resize(ref array, originalSize);
                            }
                            return o;
                        }
                        catch (Exception ex)
                        {
                            await this.CloseSocket();
                            throw new LightningPayException($"Invalid payload: {ex.Message}", LightningPayException.ErrorCode.BAD_REQUEST);
                        }
                    }
                    else
                    {
                        if (buffer.Count - message.Count <= 0)
                        {
                            newSize *= 2;
                            if (newSize > 1024 * 1024 * 5)
                            {
                                await this.CloseSocket();
                                throw new LightningPayException($"Message is too big", LightningPayException.ErrorCode.BAD_REQUEST);
                            }
                            Array.Resize(ref array, newSize);
                            buffer = new ArraySegment<byte>(array, buffer.Offset, newSize - buffer.Offset);
                        }
                        buffer = new ArraySegment<byte>(buffer.Array, buffer.Offset + message.Count, buffer.Count - message.Count);
                    }
                }
            }
            catch (Exception) when (this.cts.IsCancellationRequested)
            {
                throw new OperationCanceledException(this.cts.Token);
            }

            throw new InvalidOperationException("Should never happen");
        }

        private async Task CloseSocket()
        {
            try
            {
                if (this.socketClient.State == WebSocketState.Open)
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(5000);
                    await this.socketClient.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", cts.Token);
                }
            }
            catch { }
            finally { try { this.socketClient.Dispose(); } catch { } }
        }

        /// <summary>Stops listening the events.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        public Task StopListening()
        {
            this.cts.Cancel();

            return Task.CompletedTask;
        }


        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            if (!cts.IsCancellationRequested)
            {
                cts.Cancel();
            }
        }
    }
}
