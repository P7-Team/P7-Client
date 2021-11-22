using System.Collections;

namespace Client.Interfaces
{
    public interface IBatchInterface
    {
        public IEnumerable GetBatchStatus();

        public bool GetResult();
    }
}