
using System;
using System.Collections.Generic;
using System.Net.Http;
using Client.Interfaces;
using Newtonsoft.Json;
using System.IO;
using System;
using System.IO;
using System.Threading.Tasks;
using Client.Models;
using Task = System.Threading.Tasks.Task;
using Client.Services;


namespace Client.Clients
{
    public class BatchClient : IBatchClient
    {
        private List<BatchStatus> _batchStatuslist = new List<BatchStatus>();
        private IHttpService _service;
        static readonly HttpClient client = new HttpClient();

        public BatchClient(IHttpService service)
        {
            _service = service;
        }

        public void CopyStreamToFile(Stream stream, string destPath)
        {
            using (var fileStream = new FileStream(destPath, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(fileStream);
                fileStream.Dispose();
            }
        }

        public async Task SaveBatchResultAsync(string patchToSavefiles, string file)
        {
            try
            {

                HttpResponseMessage response = _service.Get("api/batch/result/" + file);

                if (response.IsSuccessStatusCode && response.Content != null)
                {

                    try
                    {
                        Stream filecontent = await response.Content.ReadAsStreamAsync();

                        if (Directory.Exists(patchToSavefiles))
                        {
                            string path = patchToSavefiles + "//" + file;
                            CopyStreamToFile(filecontent, path);
                        }
                    }
                    catch (UnauthorizedAccessException e)
                    {
                        Console.WriteLine("\nException Caught!");
                        Console.WriteLine("Message :{0} ", e.Message);
                        throw e;
                    }
                    catch (IOException IOE)
                    {
                        Console.WriteLine("\nException Caught!");
                        Console.WriteLine("Message :{0} ", IOE.Message);
                        throw IOE;
                    }
                    catch (ArgumentNullException Anull)
                    {
                        Console.WriteLine("\nException Caught!");
                        Console.WriteLine("Message :{0} ", Anull.Message);
                        throw Anull;
                    }
                }

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

        }

        public bool AddBatch(Batch batch)
        {
            Dictionary<string, string> formdata = new Dictionary<string, string>()
            {
                {"id", batch.Id},
                {"language", batch.Language}
            };

            Dictionary<string, Stream> files = new Dictionary<string, Stream>()
            {
                {"source", batch.Source.Data}
            };

            for (int i = 0; i < batch.Inputs.Count; i++)
            {
                files.Add(batch.Inputs[i].Name, batch.Inputs[i].Data);
                string encoding = "encoding" + batch.Inputs[i].Name;
                formdata.Add(encoding, batch.Inputs[i].Enc.BodyName);
            }

            MultipartContent content = MultipartFormDataHelper.CreateContent(formdata, files);

            HttpResponseMessage response = _service.Post("/api/batch", content);

            return response.IsSuccessStatusCode;
        }

        public List<BatchStatus> GetBatchStatus()
        {

            try
            {
                HttpResponseMessage response = _service.Get("/api/batch/status");
                List<BatchStatus> _batchStatuslist=  new List<BatchStatus>(); 

                //  If resice a success resonse 
                if (response.IsSuccessStatusCode && response.Content != null)
                {
                    try
                    {
                        _batchStatuslist = JsonConvert.DeserializeObject<List<BatchStatus>>(response.Content.ReadAsStringAsync().Result);
                    }
                    catch (ArgumentNullException e)
                    {
                        Console.WriteLine(" Something when wrong with downloading Batches Status list ", e);
                        throw e;
                    }
                    return _batchStatuslist;
                }
                else
                {
                    Console.WriteLine("BatchesStatus could not be Received from a server");
                    return _batchStatuslist;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
                throw ex;
            }
        }


        public async void GetResult(List<BatchStatus> Result, string patchToSavefiles)
        {
            if (Result.Count > 0)
            {
                foreach (BatchStatus _batchstatus in Result)
                {
                    if (_batchstatus.Finished && _batchstatus.Files.Count > 0)
                    {
                        foreach (string File in _batchstatus.Files)
                        {
                            await SaveBatchResultAsync(patchToSavefiles, File);
                        }
                    }
                }
            }
        }
    }
}


