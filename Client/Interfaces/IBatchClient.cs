using Client.Models;

namespace Client.Interfaces
{
    public interface IBatchClient
    {
        bool AddBatch(Batch batch);
    }
}