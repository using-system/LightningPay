using LightningPay.Tools;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LightningPay.Clients.CLightning
{
    public class DefaultCLightningTcpClient : ICLightningTcpClient
    {
		static Encoding UTF8 = new UTF8Encoding(false);

		public async Task<Response> SendCommandAsync<Response>(Uri address, string command, object[] parameters = null)
        {
			parameters = parameters ?? Array.Empty<string>();
			using (Socket socket = await Connect(address))
			{
				using (var networkStream = new NetworkStream(socket))
				{
					using (var textWriter = new StreamWriter(networkStream, UTF8, 1024 * 10, true))
					{
						textWriter.Write(Json.Serialize(new CommandRequest()
						{
							Id = 0,
							Command = command,
							Parameters = parameters
						}));
						await textWriter.FlushAsync();
					}
					await networkStream.FlushAsync();
					using (var textReader = new StreamReader(networkStream, UTF8, false, 1024 * 10, true))
					{
						string json = "";
						while (textReader.Peek() >= 0)
						{
							json += (char)(textReader.Read());
						}

						var response = Json.Deserialize<CommandResponse<Response>>(json, new JsonOptions()
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
