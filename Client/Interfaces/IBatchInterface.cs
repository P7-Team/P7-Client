using System.Collections.Generic;
using Client.Models;

namespace Client.Interfaces
{
    public interface IBatchInterface
    {
        public IEnumerable<Batch> GetBatchStatus();

        public bool GetResult();
    }
}