using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Client.Clients;
using Client.Interfaces;
using Client.Models;
using Newtonsoft.Json;
using Xunit;

namespace Client_app
{
    public class TaskClientTests
    {
        public class TestHttpClient : HttpClient, IHttpClient
        {
            private bool _taskIsReady;
            private HttpStatusCode _statusCode;
            public TestHttpClient(bool taskIsReady, HttpStatusCode statusCode)
            {
                _taskIsReady = taskIsReady;
                _statusCode = statusCode;
            }
            
            public HttpResponseMessage Send(HttpRequestMessage message)
            {
                HttpResponseMessage responseMessage = new HttpResponseMessage();

                if (_taskIsReady)
                {
                    Dictionary<string, string> dict = new Dictionary<string, string>()
                    {
                        {"source", "some source"},
                        {"input", "some input"}
                    };

                    string json = JsonConvert.SerializeObject(dict, Formatting.Indented);

                    responseMessage.Content = new StringContent(json);
                }
                else
                {
                    responseMessage.Content = new StringContent("");
                }

                responseMessage.StatusCode = _statusCode;
                return responseMessage;
            }
        }
        
        [Fact]
        public void SendGetRequest_TaskIsReady_RequestAnsweredWithTask()
        {
            IHttpClient testHttpClient = new TestHttpClient(true, HttpStatusCode.OK);
            TaskClient client = new TaskClient(testHttpClient);

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, "api/task/ready");
            
            Task response = client.GetTask(requestMessage);
            
            Assert.NotNull(response);
            Assert.IsType<Task>(response);
        }

        [Fact]
        public void SendGetRequest_TaskIsNotReady_RequestAnsweredWithNull()
        {
            IHttpClient testHttpClient = new TestHttpClient(false, HttpStatusCode.Forbidden);
            TaskClient client = new TaskClient(testHttpClient);

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, "api/task/ready");

            Task response = client.GetTask(requestMessage);

            Assert.Null(response);
        }
    }
}