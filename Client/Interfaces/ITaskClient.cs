using Client.Models;

namespace Client.Interfaces
{
    public interface ITaskClient
    {
        public Task GetTask();

        public bool AddResult(CompletedTask completedTask);
        
    }
}