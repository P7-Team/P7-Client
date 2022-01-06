using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Models;

namespace Client.Interfaces
{
    public interface IBatchClient
    {
        public bool AddBatch(Batch batch);
        
        public List<BatchStatus> GetBatchStatus();

        public Task<bool> GetResult(List<BatchStatus> statusList, string pathToSaveFiles);
    }
}