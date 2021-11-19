using System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Client.Clients;
using Client.Interfaces;
using Client.Models;
using Newtonsoft.Json;
using Xunit;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;


namespace Client_app
{
    public class HeartbeatClientTests 
    {
        
        private class testHttpService : IHttpService
        {
            private HttpStatusCode _statuscode;
            public testHttpService()
            {
                _statuscode = HttpStatusCode.BadRequest;
            }
            public HttpResponseMessage Send(HttpRequestMessage message)
            {
                throw new NotImplementedException();
            }
            public HttpResponseMessage Get(string uri)
            {
                throw new NotImplementedException();
            }
            public HttpResponseMessage Post(string uri, HttpContent content)
            {
                HttpResponseMessage responeMessage = new HttpResponseMessage();
                Dictionary<string, string> dictContent = JsonConvert.DeserializeObject<Dictionary<string, string>>(content.ReadAsStringAsync().Result);

                string MessageType = "";

                dictContent.TryGetValue("MessageType", out MessageType);

                if ())
                {
                    ;
                }
                else
                {
                    ;
                }

                return responeMessage;
            }
            public HttpResponseMessage Delete(string uri)
            {
                throw new NotImplementedException();
            }
        }

        //TODO - TEST: Recieve OK from method Working, Done, ShuttingDown
        [Fact]
        public void TestHearbeatRecieveOK()
        {
        

        }

        //TODO - TEST: Received Error for sending nonsense
        [Fact]
        public void TestHearbeatSend()
        {
       
            Assert.Equal(2, 2);

        }
    }
}
