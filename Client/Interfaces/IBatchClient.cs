using System.Collections.Generic;
using Client.Models;

namespace Client.Interfaces
{
    public interface IBatchClient
    {
        bool AddBatch(Batch batch);
        
        public IEnumerable<Batch> GetBatchStatus();

        public bool GetResult();
    }
}