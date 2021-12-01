using Client.Clients;
using Client.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace Client_app
{
    public class BatchClientTest
    {
        private class TestHttpService : IHttpService
        {
            private bool _finished;
            private string _batchID;
            private string _files; 
            private HttpStatusCode _statusCode;

            public TestHttpService( bool finenhed ,string batchID, string files, HttpStatusCode statusCode)
            {
                _finished = finenhed;
                _batchID = batchID;
                _files = files; 
                _statusCode = statusCode;
            }


            public HttpResponseMessage Get(string uri)
            {
                HttpResponseMessage responseMessage = new HttpResponseMessage();

                if (_finished)
                {
                    if (_files == "filename")
                    {
                        string[] fileID = { "filepath1+filename1", "filepath2+filename2" };


                        string json = JsonConvert.SerializeObject(fileID, Formatting.Indented);

                        responseMessage.Content = new StringContent(json);
                    }
                    else
                    {
                        responseMessage.Content = new StringContent("");
                    }

                }
                responseMessage.StatusCode = _statusCode;
                return responseMessage; 
            }


                public HttpResponseMessage Send(HttpRequestMessage message)
            {
                throw new NotImplementedException();
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
        public void ResultOfBatch_RequestAnsweredWithTask()
        {
            string[] files = { "testfile1", "testfile2" };
            IHttpService testHttpService = new TestHttpService(true, "somefiles","testfiles", HttpStatusCode.OK);
            BatchClient client = new BatchClient(testHttpService);

            List<BatchStatus> response = client.GetBatchStatus();

            Assert.NotNull(response);
            Assert.IsType<BatchStatus>(response);
        }
    }
}
