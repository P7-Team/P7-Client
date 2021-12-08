
using System;
using System.Collections.Generic;
using System.Net.Http;
using Client.Interfaces;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using Client.Models;
using Task = System.Threading.Tasks.Task;
using Client.Services;
using System.Linq;


namespace Client.Clients
{
    public class BatchClient : IBatchClient
    {
        // private List<BatchStatus> _batchStatusList = new List<BatchStatus>();
        private readonly IHttpService _service;

        public BatchClient(IHttpService service)
        {
            _service = service;
        }

        public void CopyStreamToFile(Stream stream, string destPath)
        {
            using (FileStream fileStream = new FileStream(destPath, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(fileStream);
            }
        }

        public async Task<bool> SaveBatchResultAsync(string pathToSaveFiles, string file)
        {
            try
            {

                HttpResponseMessage response = _service.Get("api/fileDownload/" + file);

                if (response.IsSuccessStatusCode && response.Content != null)
                {

                    try
                    {
                        Stream fileContent = await response.Content.ReadAsStreamAsync();

                        if (Directory.Exists(pathToSaveFiles))
                        {
                            string path = pathToSaveFiles + Path.DirectorySeparatorChar + file;
                            CopyStreamToFile(fileContent, path);
                            return true; 
                        }
                        else
                        {
                            return false; 
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("\nException Caught!");
                        Console.WriteLine("Message :{0} ", e.Message);
                        return false; 
                    }
                }
                else
                {
                    return false; 
                }

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return false; 
            }

        }

        public bool AddBatch(Batch batch)
        {
            Dictionary<string, string> formData = new Dictionary<string, string>()
            {
                {"ExecutableLanguage", batch.Language}
            };

            ICollection<KeyValuePair<string, Stream>> files = new List<KeyValuePair<string, Stream>>();
            {
                new KeyValuePair<string, Stream>("Executable", batch.Source.Data);
            };

            for (int i = 0; i < batch.Inputs.Count; i++)
            {
                files.Add(new KeyValuePair<string, Stream>("Input", batch.Inputs[i].Data));
                //formdata.Add(encoding, batch.Inputs[i].Enc.BodyName);
            }

            MultipartContent content = MultipartFormDataHelper.CreateContent(formData, files);

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


        public async Task<bool> GetResult(List<BatchStatus> Result, string patchToSavefiles)
        {
            if (Result.Count > 0)
            {
                foreach (BatchStatus batchstatus in Result)
                {
                    if (batchstatus.Finished && batchstatus.Files.Count > 0)
                    {
                        foreach (string File in batchstatus.Files)
                        {
                            await SaveBatchResultAsync(patchToSavefiles, File);
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


