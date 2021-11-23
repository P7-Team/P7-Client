using System.Collections.Generic;
using Client.Interfaces;
using Client.Models;

namespace Client.Clients
{
    class BatchDownloaderClient : IBatchInterface
    {
        private IHttpService _httpService;

        public BatchDownloaderClient(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public IEnumerable<Batch> GetBatchStatus()
        {
            return new List<Batch>();
        }

        public bool GetResult()
        {
            return false;
        }
    }
}