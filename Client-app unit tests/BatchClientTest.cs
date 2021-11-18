using System.Net.Http;
using Client.Interfaces;

namespace Client_app
{
    public class BatchClientTest
    {
        public class SendBatchClient : IHttpService
        {
            public HttpContent Content { get; private set; }
            
            public HttpResponseMessage Send(HttpRequestMessage message)
            {
                throw new System.NotImplementedException();
            }

            public HttpResponseMessage Get(string uri)
            {
                throw new System.NotImplementedException();
            }

            public HttpResponseMessage Post(string uri, HttpContent content)
            {
                Content = content;
                return new HttpResponseMessage();
            }

            public HttpResponseMessage Delete(string uri)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}