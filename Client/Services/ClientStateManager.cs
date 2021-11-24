using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Client.Clients;
using Client.Interfaces;
using Client.Models;

namespace Client.Services
{
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
        private readonly IHeartbeatClient _heartbeatClient;
        private readonly ITaskClient _taskClient;
        private readonly IBatchClient _batchClient;
        private Thread _heartBeatThread;
        private Thread _taskThread;
        private Thread _batchThread;
        private const int HeartbeatInMinutes = 5;
        private const int ThreadTimeout = 1000 * 60 * HeartbeatInMinutes;
        private readonly Dictionary<string, string> _config;
        
        private bool _shutdown = false;

        public ClientStateManager(IHttpService httpService)
        {
            _heartbeatClient = new HeartbeatClient(httpService);
            _taskClient = new TaskClient(httpService);
            _batchClient = new BatchClient(httpService);
            _config = new ConfigManager().GetConfig();
        }

        /// <summary>
        /// Runs the client state manager.
        /// This is responsible for running heartbeat,task and batch threads.
        /// </summary>
        public void Run()
        {
            _heartBeatThread = new Thread(HeartbeatThreadHandler);
            _taskThread = new Thread(TaskThreadHandler);
            _batchThread = new Thread(BatchThreadHandler);
            _batchThread.Start();
            _taskThread.Start();
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
            // TODO: This should loop like the other handlers. If the user has no uploaded Batches, it should not loop until the user has uploaded a Batch. 
            if (!_batchClient.GetResult())
            {
                return;
            }

            // TODO: Should be a list of batch statuses, not a list of batches
            List<Batch> batches = (List<Batch>) _batchClient.GetBatchStatus();
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
                        _heartbeatClient.SendHeartbeatWorking();
                        break;
                    case Status.Done:
                        Console.WriteLine("Sent heartbeat");
                        _heartbeatClient.SendHeartbeatDone();
                        break;
                    case Status.ShuttingDown:
                        Console.WriteLine("Sent shutdown");
                        _heartbeatClient.SendHeartbeatShuttingDown();
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
                Task task = _taskClient.GetTask();
                Console.WriteLine("I just asked for a task");
                if (task != null)
                {
                    _status = Status.Working;
                    
                    // Initiates a heartbeat thread which sends heartbeats.
                    _heartBeatThread.Start();
                    
                    // Setup the task completer and run the task
                    IInterpretedTaskCompleter interpretedTaskCompleter =
                        new InterpretedTaskCompleter(_config["InterpreterPath"], _config["WorkingDirectory"],
                            task.getSource());
                    interpretedTaskCompleter.Run();
                    
                    // TODO: The result of the Task should be sent to the service

                    // Exits the heartbeat thread
                    _heartBeatThread.Abort();
                    
                    // Sends the done message, and ensures it is received
                    bool response;
                    do
                    {
                        response = _heartbeatClient.SendHeartbeatDone();
                    } while (!response);
                    
                    _status = Status.Done;
                }

                Thread.Sleep(ThreadTimeout);
            }
        }
    }
}