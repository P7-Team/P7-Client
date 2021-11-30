using System;
using System.Collections.Generic;
using System.Net.Http;
using Client.Interfaces;
using Client.Models;
using Client.Services;
using Newtonsoft.Json;
using System.IO;
using Task = Client.Models.Task;
using Microsoft.AspNetCore.WebUtilities;

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

        static readonly HttpClient client = new HttpClient();
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


        public async void GetResult()
        {
            List<BatchStatus> Result = GetBatchStatus();
            BatchStatus _bacth1 = new BatchStatus(true, 1, 10, 1);
            BatchStatus _bacth2 = new BatchStatus(true, 1, 10, 1);
            BatchStatus _bacth3 = new BatchStatus(true, 1, 10, 1);
            Result.Add(_bacth1);
            // Result.Add(_bacth2);
            // Result.Add(_bacth3);

            int resultlength = Result.Count;

            if (resultlength > 0)
            {
                foreach (var Batchstatus in Result)
                {

                    if (Batchstatus.Finished)
                    {
                       
                        int id = Batchstatus.BatchID;
                        try
                        {
                            HttpResponseMessage response = await client.GetAsync("http://localhost:5000/api/batch/result");
                            response.EnsureSuccessStatusCode();
                            string responseBody = await response.Content.ReadAsStringAsync();
                            Stream streanmOfFiles = await response.Content.ReadAsStreamAsync(); 
                            Console.WriteLine(responseBody);
                            SectionedDataReader reader = new SectionedDataReader(new MultipartReader(responseBody, streanmOfFiles));
                            //MultipartFormDataContent  multiFile = new MultipartFormDataContent();


                            MultipartMarshaller<MultipartSection> batchMarshaller = new MultipartMarshaller<MultipartSection>(reader);

                            Dictionary<string, string> formData = batchMarshaller.GetFormData();
                            List<FileStream> streams = batchMarshaller.GetFileStreams();
                            //= (MultipartFormDataContent)response; 
                            //Console.WriteLine(multiFile.ToString());



                            //Console.Write(StreamOfFiles.ToString());
                            //Console.Read();


                        }
                        catch (HttpRequestException e)
                        {
                            Console.WriteLine("\nException Caught!");
                            Console.WriteLine("Message :{0} ", e.Message);
                            Console.Read();
                        }
                        // string pathToBacth = "/batch/result/" + id + ":";
                        // HttpResponseMessage response = _service.Get("http://localhost:5000/api/batch/result");
                        // 
                        // var fileInfo = new FileInfo($"bob.txt");
                        // var response = await _service.GetAsync(http://localhost:5000/api/batch/result);
                        // response.Content = (MultipartFormDataContent) multiFile;
                        // multiFile = (MultipartFormDataContent) response.Content;
                        // multiFile.Add((MultipartFormDataContent)response.Content);

                        
                    }
                }
            }

        }
    }
}