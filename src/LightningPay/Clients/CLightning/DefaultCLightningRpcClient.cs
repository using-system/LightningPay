using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using LightningPay.Tools;

namespace LightningPay.Clients.CLightning
{
    /// <summary>
    ///  Default  C-Lightning tcp client
    /// </summary>
    public class DefaultCLightningRpcClient : IRpcClient
    {
        private readonly Uri address;

        /// <summary>Initializes a new instance of the <see cref="DefaultCLightningRpcClient" /> class.</summary>
        /// <param name="options">The options.</param>
        public DefaultCLightningRpcClient(CLightningOptions options)
        {
            this.address = options.Address;
        }

		static Encoding UTF8 = new UTF8Encoding(false);

        /// <summary>Sends the command asynchronous.</summary>
        /// <typeparam name="Response">The type of the esponse.</typeparam>
        /// <param name="command">The command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <exception cref="LightningPay.LightningPayException">Error code {response.Error.Code} : {response.Error.Message}</exception>
        public async Task<Response> SendCommandAsync<Response>(string command, object[] parameters = null)
        {
			parameters = parameters ?? Array.Empty<string>();
			using (Socket socket = await Connect(address))
			{
				using (var networkStream = new NetworkStream(socket))
				{
					using (var textWriter = new StreamWriter(networkStream, UTF8, 1024 * 10, true))
					{
                        Json.Serialize(textWriter, new CommandRequest()
                        {
                            Id = 0,
                            Command = command,
                            Parameters = parameters
                        });
					}
					await networkStream.FlushAsync();
					using (var textReader = new StreamReader(networkStream, UTF8, false, 1024 * 10, true))
					{
						var response = Json.Deserialize<CommandResponse<Response>>(textReader, new JsonOptions()
						{
							SerializationOptions = JsonSerializationOptions.ByteArrayAsBase64
						});

						if (response.Error != null)
						{
							throw new LightningPayException(
								$"Error code {response.Error.Code} : {response.Error.Message}", LightningPayException.ErrorCode.BAD_REQUEST);
						}
						return response.Result;

					}
				}
			}
		}

        private async Task<Socket> Connect(Uri target)
        {
            EndPoint endpoint;
            Socket socket;

            if (target.Scheme == "tcp" || target.Scheme == "tcp")
            {
                var domain = target.DnsSafeHost;
                if (!IPAddress.TryParse(domain, out IPAddress address))
                {
                    address = (await Dns.GetHostAddressesAsync(domain)).FirstOrDefault();
                    if (address == null)
                        throw new Exception("Host not found");
                }
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                endpoint = new IPEndPoint(address, target.Port);
            }
            else
                throw new NotSupportedException($"Protocol {target.Scheme} for clightning not supported");

            await socket.ConnectAsync(endpoint);
            return socket;
        }
    }
}
