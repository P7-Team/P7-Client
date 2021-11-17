using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text.Json.Serialization;
using Client.Interfaces;
using Client.Models;
using Newtonsoft.Json;

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
    }
}