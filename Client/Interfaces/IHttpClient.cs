using System.Net.Http;
using System.Threading.Tasks;

namespace Client.Interfaces
{
    public interface IHttpClient
    {
        HttpResponseMessage Send(HttpRequestMessage message);

        Task<HttpResponseMessage> SendAsync(HttpRequestMessage message);
    }
}