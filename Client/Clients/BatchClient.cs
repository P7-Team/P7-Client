using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Client.Interfaces;
using Client.Models;
using Client.Services;

namespace Client.Clients
{
    public class BatchClient
    {

        private IHttpService _service;

        public BatchClient(IHttpService service)
        {
            _service = service;
        }

        public bool AddBatch(Batch batch)
        {
            Dictionary<string, string> formdata = new Dictionary<string, string>()
            {
                {"id", batch.Id}
            };

            Dictionary<string, Stream> files = new Dictionary<string, Stream>()
            {
                {"executable", batch.Executable}
            };

            for (int i = 0; i < batch.Inputs.Count; i++)
            {
                string inputName = "input" + (i+1); // to make sure the inputs are named from 1..n
                files.Add(inputName, batch.Inputs[i]);
            }

            MultipartContent content = MultipartFormDataHelper.CreateContent(formdata, files);

            HttpResponseMessage response = _service.Post("/api/batch", content);

            return response.IsSuccessStatusCode;
        }
    }
}