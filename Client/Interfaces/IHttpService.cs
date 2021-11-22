using System.Net.Http;

namespace Client.Interfaces
{
    public interface IHttpService
    {
        HttpResponseMessage Send(HttpRequestMessage message);
        HttpResponseMessage Get(string uri);
        HttpResponseMessage Post(string uri, HttpContent content);
        HttpResponseMessage Delete(string uri);

        void SetToken(string token);
    }
}