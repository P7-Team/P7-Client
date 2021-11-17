using System.Net.Http;

namespace Client.Interfaces
{
    public interface IHttpClient
    {
        HttpResponseMessage Send(HttpRequestMessage message);
    }
}