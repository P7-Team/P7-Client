using Client.Clients;
using Client.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Client.Models;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace Client_app
{
    public class BatchClientTest
    {
        
        private class TestHttpService : IHttpService
        {
            private HttpStatusCode _statusCode { get; set; }
           
            public TestHttpService(HttpStatusCode statusCode)
            {

                _statusCode = statusCode;
            }


            public HttpResponseMessage Get(string uri)
            {
                HttpResponseMessage responseMessage = new HttpResponseMessage();

                if (uri.EndsWith("result"))
                {
                    MemoryStream stringAsStream = new MemoryStream();
                    StreamWriter streamData = new StreamWriter(stringAsStream, UnicodeEncoding.UTF8);
                    streamData.Write("this is data in result of one batch");
                    stringAsStream.Position = 0L;
                    responseMessage.Content = new StringContent(streamData.ToString());
                }

                if (uri.EndsWith("status"))
                {
                    if (_statusCode == HttpStatusCode.OK) 
                    { 
                        BatchStatus batchStatus = new BatchStatus(true, 1, 5, 10);
                        BatchStatus batchStatus1 = new BatchStatus(false, 2, 1, 2);
                        BatchStatus batchStatus2 = new BatchStatus(true, 3, 2, 2);
                        BatchStatus batchStatus3 = new BatchStatus(false, 4, 3, 5);

                        batchStatus.Files.Add("testFile");
                        batchStatus2.Files.Add("testFile1");
                        batchStatus2.Files.Add("testFile2");

                        List<BatchStatus> testBatchStatuses = new List<BatchStatus>
                        {
                            batchStatus,
                            batchStatus1,
                            batchStatus2,
                            batchStatus3
                        };

                        string json = JsonConvert.SerializeObject(testBatchStatuses, Formatting.Indented);
                        responseMessage.Content = new StringContent(json);
                    }
                    else
                    {
                        responseMessage.Content = new StringContent("");
                    }
                }
                

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

            public void SetToken(string token)
            {
                throw new NotImplementedException();
            }
        }
        [Fact]
        public Task GetFileRuns()
        {
            IHttpService testHttpService = new TestHttpService(HttpStatusCode.OK);
            BatchClient client = new BatchClient(testHttpService);

            Task response = client.SaveBatchResultAsync("path","filename");

            Assert.NotNull(response);
            return Task.CompletedTask;
        }

        //<Tested Method>_(Optional)<Precondition>_<Expected Result>
        [Fact]
        public void GetBatces_BatchesIsAvailable_BatchesReceived()
        {
            IHttpService testHttpService = new TestHttpService(HttpStatusCode.OK);
            BatchClient client = new BatchClient(testHttpService);

            List<BatchStatus> response = client.GetBatchStatus();
            Assert.True(response.Count > 0);
            Assert.IsType<BatchStatus>(response[0]);
        }

        [Fact]
        public void GetBatch_EnsureFourDifferent()
        {
            IHttpService testHttpService = new TestHttpService(HttpStatusCode.OK);
            BatchClient client = new BatchClient(testHttpService);

            List<BatchStatus> response = client.GetBatchStatus();
            Assert.NotNull(response);
            Assert.NotSame(response[0], response[1]);
            Assert.NotSame(response[1], response[2]);
            Assert.NotSame(response[2], response[3]);
        }

        [Fact]
        public void GetBatchesStatus_ResponsContainsFourBatches()
        {
            IHttpService testHttpService = new TestHttpService(HttpStatusCode.OK);
            BatchClient client = new BatchClient(testHttpService);
            List<BatchStatus> response = client.GetBatchStatus();

            Assert.True(response.Count == 4 );

        }
    }
}
