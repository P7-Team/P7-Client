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
        public Task GetTask()
        {
            HttpResponseMessage response = _service.Get("/api/task/ready");

            if (response.IsSuccessStatusCode)
            {
                var values =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content.ReadAsStringAsync()
                        .Result);

                var task = new Task(values["source"], values["input"]);

                return task;
            }

            return null;
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
                {"id", completedTask.Id.ToString()}
            };
            Dictionary<string, Stream> files = new Dictionary<string, Stream>()
            {
                {"result", completedTask.FileStream}
            };

            MultipartFormDataContent content = MultipartFormDataHelper.CreateContent(formdata, files);

            return _service.Post("/task/complete", content).IsSuccessStatusCode;
        }
    }
}