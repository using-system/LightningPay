using System;

using System.Threading.Tasks;

namespace LightningPay.Clients.CLightning
{
    public interface ICLightningTcpClient
    {
        Task<Response> SendCommandAsync<Response>(Uri address,  string command, object[] parameters = null);
    }
}
