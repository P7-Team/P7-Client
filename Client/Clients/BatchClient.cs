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
                {"source", batch.Source.Data}
            };

            for (int i = 0; i < batch.Inputs.Count; i++)
            {
                files.Add(batch.Inputs[i].Name, batch.Inputs[i].Data);
            }

            MultipartContent content = MultipartFormDataHelper.CreateContent(formdata, files);

            HttpResponseMessage response = _service.Post("/api/batch", content);

            return response.IsSuccessStatusCode;
        }
    }
}