using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Client.Clients;
using Client.Interfaces;
using Newtonsoft.Json;
using Xunit;

namespace Client_app
{
    public class HeartbeatClientTests 
    {
        
        private class TestHttpService : IHttpService
        {
            private readonly bool _connection;
            private HttpStatusCode _statuscode;
            private readonly HttpResponseMessage _responeMessage = new HttpResponseMessage();

            public TestHttpService(bool isThereAConnection)
            {
                _connection = isThereAConnection;
                _statuscode = HttpStatusCode.BadRequest;
                _responeMessage.StatusCode = _statuscode;
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
                if (_connection)
                {
                    Dictionary<string, string> dictContent = JsonConvert.DeserializeObject<Dictionary<string, string>>(content.ReadAsStringAsync().Result);
                    string found;

                    if (dictContent.TryGetValue("status", out found) && (found == "Working" | found == "Done" | found == "ShuttingDown"))
                    {
                        _statuscode = HttpStatusCode.OK;
                    }
                    else if (dictContent.Count == 0)
                    {
                        _statuscode = HttpStatusCode.NoContent;
                    }
                    else
                    {
                        _statuscode = HttpStatusCode.BadRequest;
                    }

                    _responeMessage.StatusCode = _statuscode;
                    return _responeMessage;
                }
                else
                {
                    _responeMessage.StatusCode = HttpStatusCode.ServiceUnavailable;
                    return _responeMessage;
                }
            }
            public HttpResponseMessage Delete(string uri)
            {
                throw new NotImplementedException();
            }

            public void SetToken(string token)
            {
                throw new NotImplementedException();
            }
        }

       
        [Fact]
        public void TestHearbeatPostDoneRecieveOK()
        {
            IHttpService testHttpService = new TestHttpService(true);
            HeartbeatClient testHeartbeatClient = new HeartbeatClient(testHttpService);

            bool response;

            response = testHeartbeatClient.SendHeartbeatDone();
            Assert.True(response);
        }

        [Fact]
        public void TestHearbeatPostShuttingDownRecieveOK()
        {
            IHttpService testHttpService = new TestHttpService(true);
            HeartbeatClient testHeartbeatClient = new HeartbeatClient(testHttpService);

            bool response;

            response = testHeartbeatClient.SendHeartbeatShuttingDown();
            Assert.True(response);
        }

        [Fact]
        public void TestHearbeatPostWorkingRecieveOK()
        {
            IHttpService testHttpService = new TestHttpService(true);
            HeartbeatClient testHeartbeatClient = new HeartbeatClient(testHttpService);

            bool response;

            response = testHeartbeatClient.SendHeartbeatWorking();
            Assert.True(response);
        }

        [Fact]
        public void TestHeartbeatPostBadConnectionDone()
        {
            IHttpService testHttpService = new TestHttpService(false);
            HeartbeatClient testHeartbeatClient = new HeartbeatClient(testHttpService);

            bool response;

            response = testHeartbeatClient.SendHeartbeatWorking();
            Assert.False(response);
        }

        [Fact]
        public void TestHeartbeatBadConnectionShuttingDown()
        {
            IHttpService testHttpService = new TestHttpService(false);
            HeartbeatClient testHeartbeatClient = new HeartbeatClient(testHttpService);

            bool response;

            response = testHeartbeatClient.SendHeartbeatShuttingDown();
            Assert.False(response);
        }

        [Fact]
        public void TestHeartbeatBadConnectionWorking()
        {
            IHttpService testHttpService = new TestHttpService(false);
            HeartbeatClient testHeartbeatClient = new HeartbeatClient(testHttpService);

            bool response;

            response = testHeartbeatClient.SendHeartbeatWorking();
            Assert.False(response);
        }
    }
}
