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
    }
}