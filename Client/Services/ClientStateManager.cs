using System;
using System.Threading.Tasks;
using Client.Interfaces;

namespace Client.Services
{
    // TODO Make Async
    // TODO SetUP basic UI.
    public class ClientStateManager
    {
        private string _token = "";
        private IHttpService _httpService;
        private ITaskClient _taskClient;
        private IInterpretedTaskCompleter _interpretedTaskCompleter;
        private IHeartbeatController _heartbeatController;

        public ClientStateManager(IHttpService httpService, ITaskClient taskClient,
            IInterpretedTaskCompleter interpretedTaskCompleter, IHeartbeatController heartbeatController)
        {
            _httpService = httpService;
            _taskClient = taskClient;
            _interpretedTaskCompleter = interpretedTaskCompleter;
            _heartbeatController = heartbeatController;
        }

        public async Task Run(string token)
        {
            _token = token;
            _taskClient.GetTask();
        }
    }
}