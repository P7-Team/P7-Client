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
        private IHttpClient _client;
        public TaskClient(IHttpClient client)
        {
            _client = client;
        }

        public Task GetTask(HttpRequestMessage message)
        {
            HttpResponseMessage response = _client.Send(message);

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
            HttpResponseMessage res = _client.Send(new HttpRequestMessage() { Content = content });

            return res.IsSuccessStatusCode;
        }
    }
}
