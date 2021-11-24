using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading;
using Client.Clients;
using Client.Interfaces;
using Client.Models;

namespace Client.Services
{
    // TODO sync working status.
    // TODO Swap BatchDownloaderClient with newest implementation after merge.

    enum Status
    {
        Working,
        Done,
        ShuttingDown
    }

    public class ClientStateManager
    {
        private Status _status;
        private readonly ITaskClient _taskClient;
        private readonly IHeartbeatController _heartbeatController;
        private Thread _heartBeatThread;
        private Thread _taskThread;
        private Thread _batchThread;
        private const int HeartbeatInMinutes = 5;
        private const int ThreadTimeout = 100 * 60 * HeartbeatInMinutes;
        private readonly Dictionary<string, string> _config;
        private IBatchInterface _batchInterface;
        private bool _shutdown = false;

        public ClientStateManager(IHttpService httpService)
        {
            _taskClient = new TaskClient(httpService);
            _heartbeatController = new HeartbeatClient(httpService);
            ConfigManager configManager = new ConfigManager();
            _batchInterface = new BatchDownloaderClient(httpService);
            _config = configManager.GetConfig();
        }

        /// <summary>
        /// Runs the client state manager.
        /// This is responsible for running heartbeat-,task- and batchthreads.
        /// </summary>
        public void Run()
        {
            _heartBeatThread = new Thread(HeartbeatThreadHandler);
            _taskThread = new Thread(TaskThreadHandler);
            _batchThread = new Thread(BatchThreadHandler);
            _batchThread.Start();
            _taskThread.Start();
            while (!_shutdown)
            {
            }
        }

        /// <summary>
        /// Initiates shutdown of all threads.
        /// </summary>
        public void Shutdown()
        {
            _shutdown = true;
        }


        /// <summary>
        /// Handles the downloading of completed tasks.
        /// </summary>
        private void BatchThreadHandler()
        {
            if (!_batchInterface.GetResult())
            {
                return;
            }

            List<Batch > batches = (List<Batch >) _batchInterface.GetBatchStatus();
            // TODO handle downloaded batches.
            if (batches.Count > 0)
            {
            }

            Thread.Sleep(ThreadTimeout);
        }

        /// <summary>
        /// Runs the heartbeat controller when working
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Throws this exception if an undefined status is provided.</exception>
        private void HeartbeatThreadHandler()
        {
            while (!_shutdown)
            {
                switch (_status)
                {
                    case Status.Working:
                        Console.WriteLine("Sent working");
                        _heartbeatController.SendHeartbeatWorking();
                        break;
                    case Status.Done:
                        Console.WriteLine("Sent working");
                        _heartbeatController.SendHeartbeatDone();
                        break;
                    case Status.ShuttingDown:
                        Console.WriteLine("Sent working");
                        _heartbeatController.SendHeartbeatShuttingDown();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                Thread.Sleep(ThreadTimeout);
            }
        }

        /// <summary>
        /// Handles the fetching and running of tasks.
        /// </summary>
        private void TaskThreadHandler()
        {
            while (!_shutdown)
            {
                Models.Task task = _taskClient.GetTask();
                Console.WriteLine("I just asked for a task");
                if (task != null)
                {
                    _status = Status.Working;
                    // Initiates a heartbeat thread which sends heartbeats.
                    _heartBeatThread.Start();
                    IInterpretedTaskCompleter interpretedTaskCompleter =
                        new InterpretedTaskCompleter(_config["InterpreterPath"], _config["WorkingDirectory"],
                            task.getSource());
                    interpretedTaskCompleter.Run();
                    // Exits the heartbeat thread
                    _heartBeatThread.Abort();
                    // Sends the done message, to ensure it gets sent 
                    _heartbeatController.SendHeartbeatDone();
                    _status = Status.Done;
                }

                Thread.Sleep(ThreadTimeout);
            }
        }
    }
}