using Client.Models;

namespace Client.Interfaces
{
    public interface ITaskClient
    {
        public System.Threading.Tasks.Task<Task> GetTask(string directory);

        public bool AddResult(CompletedTask completedTask);
        
    }
}