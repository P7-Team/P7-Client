using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Client.Interfaces;
using Client.Models;
using Client.Services;
using Newtonsoft.Json;
using Task = Client.Models.Task;

namespace Client.Clients
{
    public class TaskClient : ITaskClient
    {
        private IHttpService _service;

        public TaskClient(IHttpService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets a task from the webservice.
        /// </summary>
        /// <returns>Returns the task if one is available, returns null if there is none available.</returns>
        public async System.Threading.Tasks.Task<Task> GetTask(string workingDirectory)
        {
            HttpResponseMessage response = _service.Get("api/task/ready");

            if (response.IsSuccessStatusCode)
            {
                var values =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content.ReadAsStringAsync()
                        .Result);

                var isSuccess = await DownloadFile(values["source"], workingDirectory, "source") && 
                                await DownloadFile(values["input"], workingDirectory, "input");

                if (!isSuccess)
                {
                    return null;
                }

                return new Task("source", values["id"], values["number"], values["subNumber"]);
            }

            return null;
        }

        private async System.Threading.Tasks.Task<bool> DownloadFile(string fileName, string workingDirectory, string outputName)
        {
            HttpResponseMessage response = _service.Get("api/fileDownload/" + fileName);
            
            if (response.IsSuccessStatusCode && response.Content != null)
            {
                try
                {
                    Stream content = await response.Content.ReadAsStreamAsync();

                    if (Directory.Exists(workingDirectory))
                    {
                        string filePath = workingDirectory + Path.DirectorySeparatorChar + outputName;

                        CopyStreamToFile(content, filePath);

                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            return false;
        } 
        
        public void CopyStreamToFile(Stream stream, string destPath)
        {
            using (FileStream fileStream = new FileStream(destPath, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(fileStream);
            }
        }

        /// <summary>
        /// Converts a completed task to multipartFormDataContent and posts it to the webservice
        /// </summary>
        /// <param name="completedTask"></param>
        /// <returns>Returns the status of the request.</returns>
        public bool AddResult(CompletedTask completedTask)
        {
            Dictionary<string, string> formdata = new Dictionary<string, string>()
            {
                {"BatchId", completedTask.Id},
                {"TaskNumber", completedTask.Number},
                {"TaskSubNumber", completedTask.SubNumber}
            };
            Dictionary<string, Stream> files = new Dictionary<string, Stream>()
            {
                {"Result", completedTask.FileStream}
            };

            MultipartFormDataContent content = MultipartFormDataHelper.CreateContent(formdata, files);

            return _service.Post("api/task/complete", content).IsSuccessStatusCode;
        }
    }
}