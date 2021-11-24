using System.Net.Http;

namespace Client.Interfaces
{
    public interface IHttpService
    {
        public HttpResponseMessage Send(HttpRequestMessage message);
        public HttpResponseMessage Get(string uri);
        public HttpResponseMessage Post(string uri, HttpContent content);
        public HttpResponseMessage Delete(string uri);

        public void SetToken(string token);
    }
}