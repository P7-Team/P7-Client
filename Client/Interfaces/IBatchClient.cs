using System.Collections.Generic;
using Client.Models;

namespace Client.Interfaces
{
    public interface IBatchClient
    {
        public bool AddBatch(Batch batch);
        
        public List<BatchStatus> GetBatchStatus();

        public void GetResult(List<BatchStatus> Result, string patchToSavefiles);
    }
}