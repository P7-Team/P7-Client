using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Client.Interfaces;
using Client.Models;
using Client.Services;
using Newtonsoft.Json;
using Task = Client.Models.Task;


namespace Client.Clients
{

    public class BatchClient
    {
        List<BatchStatus> BatchStatuslist = new List<BatchStatus> { };
        // -Figure out and implement how the result should be received: small files, or a big stream, 
        //   or something else?
        // small files Alex say i will get this 
        // - Ask the web service for the result of the user's Batch
        // UserHashBatchID to find the file for download 
        // try case server return server not albeel 
        // 
        // -Extract the result from the response
        // - Covent stream into file patch 
        // - For Windoes and Linux . 

        private IHttpService _service;
        public BatchClient(IHttpService service)
        {
            _service = service;
        }


        public List<BatchStatus> GetBatchStatus()
        {
            // send get to http/batch/status:  
            HttpResponseMessage response = _service.Get("http://localhost:5000/api/batch/status");

            //  If resice a success resonse 
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    BatchStatuslist = JsonConvert.DeserializeObject<List<BatchStatus>>(response.Content.ReadAsStringAsync().Result);
                }
                catch (ArgumentNullException e)
                {
                    Console.WriteLine(".something wrong with BatchStatuslist ", e);
                }
                return BatchStatuslist;
            }
            else
            {
                Console.WriteLine("BatchesStatus could not be Received from a server");
                return BatchStatuslist;
            }
        }


        public bool GetResult()
        {
            List<BatchStatus> Result = GetBatchStatus();
            BatchStatus _bacth1 = new BatchStatus(true, 1, 10, 1);
            Result.Add(_bacth1);

            int resultlength = Result.Count;

            if (resultlength > 0)
            {
                foreach (var Batchstatus in Result)
                {

                    if (Batchstatus.Finished)
                    {
                        int id = Batchstatus.BatchID;
                        byte[] buffer = new byte[512];
                        int bytesRead = 0; 

                        string pathToBacth = "/batch/result/" + id + ":";
                        HttpResponseMessage response = _service.Get("http://localhost:5000/api/batch/result");

                        if (response.IsSuccessStatusCode)
                        {
                            do {
                                // TODO insert stream; 
                                Console.WriteLine(response);
                                // TODO make some new logi for this; 
                                // start make manu call;
                        } while (bytesRead > 0);
                        }

                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}