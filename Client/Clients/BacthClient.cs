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
using Services.MultipartFormDataHelper


namespace Client.Clients
{
   
    public class BatchClient
    {    
        list<BatchStatus> BatchStatuslist = new List<BatchStatus> 
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
        public TaskClient(IHttpService service)
        {
            _service = service;
        }
       

        public List GetBatchStatus()
        {
            // send get to http  /batch/status:  
            HttpResponseMessage response = _service.Get("/batch/status");

            //  If resice a success resonse 
            if(response.IsSuccessStatusCode)
            {
                //modetager en list af BatchStatus 
                return BatchStatuslist = JsonConvert.DeserializeObject<List<BatchStatus>>(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                Console.WriteLine("BatchesStatus could not be Received from a server")
            }
        }


        public bool GetResult()
        {
            List <BatchStatus> Result = GetBatchStatus(); 
            
            if(!Result.Count == 0)
            {                
                foreach (Batchstatus in Result)
                {
                    if (Batchstatus.Finished.Get())
                    {
                        int id = Batchstatus.BatchID.Get();
                        string pathToBacth = "/batch/result/"+id+":";  

                        HttpResponseMessage response = _service.Get(pathToBacth);

                        if(response.IsSuccessStatusCode)
                        {
                            // Dictionary<string, string>(), files)
                            List<string> binaryString = JsonConvert.DeserializeObject<List<string>>(response.Content.ReadAsStringAsync().Result);
                            // TODO make some new logi for this 
                            // start make manu call                         } 
                
                    }
                }
            }
            else 
            {
            return false; 
            }
        }

    }
}