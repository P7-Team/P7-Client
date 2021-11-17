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

namespace Client_app
{
    public class TaskClientTests
    {
        private class TestHttpService : IHttpService
        {
            private bool _taskIsReady;
            private HttpStatusCode _statusCode;
            public TestHttpService(bool taskIsReady, HttpStatusCode statusCode)
            {
                _taskIsReady = taskIsReady;
                _statusCode = statusCode;
            }
            
            public HttpResponseMessage Send(HttpRequestMessage message)
            {
                throw new NotImplementedException();
            }

            public HttpResponseMessage Get(string uri)
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

            public HttpResponseMessage Post(string uri, HttpContent content)
            {
                throw new NotImplementedException();
            }

            public HttpResponseMessage Delete(string uri)
            {
                throw new NotImplementedException();
            }
        }
        
        [Fact]
        public void SendGetRequest_TaskIsReady_RequestAnsweredWithTask()
        {
            IHttpService testHttpService = new TestHttpService(true, HttpStatusCode.OK);
            TaskClient client = new TaskClient(testHttpService);
            
            Task response = client.GetTask();
            
            Assert.NotNull(response);
            Assert.IsType<Task>(response);
        }

        [Fact]
        public void SendGetRequest_TaskIsNotReady_RequestAnsweredWithNull()
        {
            IHttpService testHttpService = new TestHttpService(false, HttpStatusCode.Forbidden);
            TaskClient client = new TaskClient(testHttpService);

            Task response = client.GetTask();

            Assert.Null(response);
        }


        /* Tests for SendCompletedTask  */
        public class MockClient : IHttpClient
        {
            public HttpRequestMessage Request { get; set; }
            public HttpResponseMessage Send(HttpRequestMessage message)
            {
                Request = message;
                return new HttpResponseMessage();
            }
        }

        [Fact]
        public void SendCompletedTask_CreatesRequestWithMultipartFormDataContentType()
        {
            MockClient httpClient = new MockClient();
            TaskClient client = new TaskClient(httpClient);

            CompletedTask completedTask = new CompletedTask(1, new MemoryStream());

            client.AddResult(completedTask);

            Assert.Equal("multipart/form-data", httpClient.Request.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async void SendCompletedTask_CreatesRequestContainingCompletedTaskData()
        {
            MockClient httpClient = new MockClient();
            TaskClient client = new TaskClient(httpClient);

            MemoryStream stream = new MemoryStream();
            stream.Write(Encoding.UTF8.GetBytes("test"));

            CompletedTask completedTask = new CompletedTask(1, stream);

            client.AddResult(completedTask);

            Assert.Contains("Content-Disposition: form-data; name=id", httpClient.Request.Content.ReadAsStringAsync().Result);
            Assert.Contains("test", await httpClient.Request.Content.ReadAsStringAsync());
        }
    }
}