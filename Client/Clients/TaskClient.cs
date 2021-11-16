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

        public async Task<bool> AddResult(long taskId, string resultFilePath)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            content.Add(new StringContent(taskId.ToString()), "id");

            using (FileStream result = File.Open(resultFilePath, FileMode.Open))
            {
                byte[] buffer = new byte[result.Length];
                await result.ReadAsync(buffer, 0, (int)result.Length);

                content.Add(new ByteArrayContent(buffer, 0, buffer.Length), "result");
            }

            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/api/task/complete");
            message.Content = content;

            HttpResponseMessage response = await _client.SendAsync(message);

            return true;
        }
        
        public void SendCompletedTask(CompletedTask completedTask)
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
            _client.Send(new HttpRequestMessage() { Content = content });
        }
    }
}
