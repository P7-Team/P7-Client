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
    public class TaskClient
    {
        private IHttpService _service;
        public TaskClient(IHttpService service)
        {
            _service = service;
        }

        public Task GetTask()
        {
            HttpResponseMessage response = _service.Get("/api/task/ready");

            if (response.IsSuccessStatusCode)
            {
                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content.ReadAsStringAsync().Result);

                var task = new Task(values["source"], values["input"]);

                return task;
            }

            return null;
        }
        
        public bool AddResult(CompletedTask completedTask)
        {
            Dictionary<string, string> formdata = new Dictionary<string, string>()
            {
                {"id", completedTask.Id.ToString() }
            };
            Dictionary<string, Stream> files = new Dictionary<string, Stream>()
            {
                { "result", completedTask.FileStream }
            };

            MultipartFormDataContent content = MultipartFormDataHelper.CreateContent(formdata, files);
            // TODO: Refactor to use the new Post after merging
            HttpResponseMessage res = _service.Send(new HttpRequestMessage() { Content = content });

            return res.IsSuccessStatusCode;
        }
    }
}
