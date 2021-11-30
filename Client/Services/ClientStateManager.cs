using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
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
        private static ClientStateManager _instance;
        
        private Status _status;
        private readonly IHeartbeatClient _heartbeatClient;
        private readonly ITaskClient _taskClient;
        private readonly IBatchClient _batchClient;
        private Thread _heartBeatThread;
        private Thread _taskThread;
        private Thread _batchThread;
        private const int HeartbeatInMinutes = 5;
        private const int HeartbeatTimeout = 1000 * 60 * HeartbeatInMinutes;
        private const int BatchTimeout = 1000 * 60 * 2;
        private const int TaskTimeout = (1000 * 60) / 4;
        private readonly Dictionary<string, string> _config;
        
        private bool _shutdown = false;
        private bool _working = false;
        private bool _heartbearting = false;
        private bool _fetchingBatches = false;

        private Task _currentTask = null;

        public static ClientStateManager GetClientStateManager()
        {
            if (_instance == null)
            {
                _instance = new ClientStateManager(HttpService.GetHttpService());
            }

            return _instance;
        }

        protected ClientStateManager(IHttpService httpService)
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
            _heartBeatThread = new Thread(HeartbeatThreadHandler)
            {
                IsBackground = true
            };
            _taskThread = new Thread(TaskThreadHandler)
            {
                IsBackground = true
            };
            _batchThread = new Thread(BatchThreadHandler)
            {
                IsBackground = true
            };
            _working = false;
            _heartbearting = false;
            _fetchingBatches = true;
            
            _taskThread.Start();
            _heartBeatThread.Start();
            _batchThread.Start();
        }

        /// <summary>
        /// Initiates shutdown of all threads.
        /// </summary>
        public void Shutdown()
        {
            _shutdown = true;
            _working = false;
            _heartbearting = false;
            _fetchingBatches = false;
            
            _batchThread?.Interrupt();
            _heartBeatThread?.Interrupt();
            _taskThread?.Interrupt();
        }
        
        /// <summary>
        /// Handles the downloading of completed tasks.
        /// </summary>
        private void BatchThreadHandler()
        {
            while (!_shutdown)
            {
                while (_fetchingBatches)
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

                    try
                    {
                        Thread.Sleep(BatchTimeout);
                    }
                    catch (ThreadInterruptedException e)
                    {
                        break;
                    }
                }

                try
                {
                    Thread.Sleep(Timeout.Infinite);
                }
                catch (ThreadInterruptedException e)
                {
                    // Left empty
                }
            }
        }

        /// <summary>
        /// Runs the heartbeat controller when working
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Throws this exception if an undefined status is provided.</exception>
        private void HeartbeatThreadHandler()
        {
            while (!_shutdown)
            {
                while (_heartbearting)
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

                    try
                    {
                        Thread.Sleep(HeartbeatTimeout);
                    }
                    catch (ThreadInterruptedException e)
                    {
                        break;
                    }
                }

                try
                {
                    Thread.Sleep(Timeout.Infinite);
                }
                catch (ThreadInterruptedException e)
                {
                    // Left empty
                }
            }
        }

        /// <summary>
        /// Handles the fetching and running of tasks.
        /// </summary>
        private void TaskThreadHandler()
        {
            while (!_shutdown)
            {
                while (_working)
                {
                    _currentTask = _taskClient.GetTask();
                    Console.WriteLine("I just asked for a task");
                    if (_currentTask != null)
                    {
                        _status = Status.Working;
                    
                        // Initiates a heartbeat thread which sends heartbeats.
                        _heartbearting = true;
                        _heartBeatThread.Interrupt();

                        // TODO: Task completer could be in its own thread
                        
                        // Setup the task completer and run the task
                        IInterpretedTaskCompleter interpretedTaskCompleter =
                            new InterpretedTaskCompleter(_config["InterpreterPath"], _config["WorkingDirectory"],
                                _currentTask.getSource());
                        interpretedTaskCompleter.Run();
                    
                        _status = Status.Done;
                    
                        // TODO: The result of the Task should be sent to the service

                        // Exits the heartbeat thread
                        _heartbearting = false;
                        _heartBeatThread.Interrupt();

                        // Sends the done message, and ensures it is received
                        bool response = false;
                        do
                        {
                            try
                            {
                                Thread.Sleep(HeartbeatTimeout);
                            }
                            catch (ThreadInterruptedException e)
                            {
                                break;
                            }
                            response = _heartbeatClient.SendHeartbeatDone();
                        } while (!response);
                        
                        // Resets the current task to null
                        _currentTask = null;
                    
                        // Tells subscribers (the UI's view model) that the task is done
                        RaiseFinishedWorking();
                        
                        if (!response)
                        {
                            break;
                        }
                        
                        try
                        {
                            Thread.Sleep(TaskTimeout);
                        }
                        catch (ThreadInterruptedException e)
                        {
                            break;
                        }
                    }
                    else
                    {
                        try
                        {
                            Thread.Sleep(TaskTimeout);
                        }
                        catch (ThreadInterruptedException e)
                        {
                            break;
                        }
                    }
                }

                try
                {
                    Thread.Sleep(Timeout.Infinite);
                }
                catch (ThreadInterruptedException e)
                {
                    // Left empty
                }
            }
            
            
        }
        
        public void StartWorking()
        {
            _working = true;
            _taskThread.Interrupt();
        }

        public void StopWorkingNow()
        {
            _working = false;
            _taskThread.Interrupt();
        }

        public void StopWorkingAfterThisTask()
        {
            _working = false;
            _taskThread.Interrupt();
        }
        
        public event Action FinishedWorking = delegate {  };

        private void RaiseFinishedWorking()
        {
            FinishedWorking();
        }

        public Task GetCurrentTask()
        {
            return _currentTask;
        }
    }
}