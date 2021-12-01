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
using System.Net;

namespace Client.Clients
{
    public class BatchClient
    {
        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
        public void CopyStreamToFile(Stream stream, string destPath)
        {
            using (var fileStream = new FileStream(destPath, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(fileStream);
                fileStream.Dispose();
            }
        }


        List<BatchStatus> BatchStatuslist = new List<BatchStatus> { };
       
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
            string[] patharrays = { "C:\\Test2\\cake1.txt", "C:\\Test2\\cake1.txt" };
            BatchStatus _bacth1 = new BatchStatus(true, 1, 10, 1, patharrays);  
            //BatchStatus _bacth2 = new BatchStatus(true, 1, 10, 1, "C:\\Test2\\cake2.txt");
            //BatchStatus _bacth3 = new BatchStatus(true, 1, 10, 1, "C:\\Test2\\cake4.txt");
            Result.Add(_bacth1);
            // Result.Add(_bacth2);
            // Result.Add(_bacth3);

            int resultlength = Result.Count;

            if (resultlength > 0)
            {
                foreach (BatchStatus _batchstatus in Result)
                {

                    if (_batchstatus.Finished)
                    {
                        for (int i = 0; i < _batchstatus.Files.Length; i++)
                        {
                            string File = _batchstatus.Files[i];
                            
                            try

                            {
                                HttpResponseMessage response = _service.Get("http://localhost:5000/api/batch/{File}");

                                if (response.IsSuccessStatusCode)
                                {
                                    Stream filecontent = await response.Content.ReadAsStreamAsync();
                                    string path = "C:\\Test2\\{[i]} out of{_batchstatus.TotalTasks} ";
                                    CopyStreamToFile(filecontent, path);
                                }

                                Console.Read();
                            }
                            catch (HttpRequestException e)
                            {
                                Console.WriteLine("\nException Caught!");
                                Console.WriteLine("Message :{0} ", e.Message);
                                Console.Read();
                            }
                        }
                        // var response = await _service.GetAsync(http://localhost:5000/api/batch/result);
                       
                    }
                }
            }

        }
    }
}