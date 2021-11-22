using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Client.Clients;
using Client.Interfaces;

namespace Client.Services
{
    // TODO Make Async
    // TODO sync working status.

    enum Status
    {
        Working,
        Done,
        ShuttingDown
    }

    public class ClientStateManager
    {
        private Status _status;
        private ITaskClient _taskClient;
        private IHeartbeatController _heartbeatController;
        private Thread _heartBeatThread;
        private Thread _taskThread;
        private Thread _batchThread;
        private IBatchInterface _batchInterface;
        private const int HeartbeatInMinutes = 5;
        private const int ThreadTimeout = 100 * 60 * HeartbeatInMinutes;
        private string _interpreterPath;
        private ConfigManager _configManager;
        private Dictionary<string, string> _config;

        public ClientStateManager(IHttpService httpService)
        {
            _taskClient = new TaskClient(httpService);
            _heartbeatController = new HeartbeatClient(httpService);
            _configManager = new ConfigManager();
            _config = _configManager.GetConfig();
        }

        public void Run()
        {
            _heartBeatThread = new Thread(Heartbeat);
            _taskThread = new Thread(TaskHandler);
            _batchThread = new Thread(BatchHandler);
            _batchThread.Start();
            _heartBeatThread.Start();
            _taskThread.Start();
            while (true)
            {
                
            }
        }

        private void BatchHandler()
        {
            // TODO implementMe
            
        }

        private void Heartbeat()
        {
            while (true)
            {
                switch (_status)
                {
                    case Status.Working:
                        _heartbeatController.SendHeartbeatWorking();
                        break;
                    case Status.Done:
                        _heartbeatController.SendHeartbeatDone();
                        break;
                    case Status.ShuttingDown:
                        _heartbeatController.SendHeartbeatShuttingDown();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                Thread.Sleep(ThreadTimeout);
            }
        }

        private void TaskHandler()
        {
            while (true)
            {
                Models.Task task = _taskClient.GetTask();
                if (task != null)
                {
                    IInterpretedTaskCompleter interpretedTaskCompleter =
                        new InterpretedTaskCompleter(_config["InterpreterPath"], _config["WorkingDirectory"],
                            task.getSource());
                    interpretedTaskCompleter.Run();
                }

                Thread.Sleep(ThreadTimeout);
            }
        }
    }
}