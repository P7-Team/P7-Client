using System.ComponentModel;
using System.Threading;
using System.Windows.Input;
using Client.Models;
using Client.Services;
using GUI.Helpers;

namespace GUI.ViewModels.Providing
{
    public class WorkingViewModel : ViewModelBase
    {
        private ClientStateManager _stateManager;

        private Thread _getCurrentTaskThread;

        public WorkingViewModel()
        {
            _stateManager = ClientStateManager.GetClientStateManager();

            _stateManager.FinishedWorking += TaskHasFinished;
            _stateManager.StoppedWorking += StoppedWorking;
            
            _stopWorkingNowCommand = new DelegateCommand(OnStopWorkingNow);

            StopWorkingAfterThisTask = false;
            
            _stateManager.StartWorking();

            GetCurrentTask();
            
            // Start a session timer??
        }

        public ICommand StopWorkingNowCommand => _stopWorkingNowCommand;
        private readonly DelegateCommand _stopWorkingNowCommand;

        private string _batchTitle;
        public string BatchTitle
        {
            get => _batchTitle;
            set => SetProperty(ref _batchTitle, value);
        }

        private string _batchDescription;
        public string BatchDescription
        {
            get => _batchDescription;
            set => SetProperty(ref _batchDescription, value);
        }

        private string _batchUser;
        public string BatchUser
        {
            get => _batchUser;
            set => SetProperty(ref _batchUser, value);
        }

        private string _status;
        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }
        
        private Task _currentTask;
        public Task CurrentTask
        {
            get => _currentTask;
            set => SetProperty(ref _currentTask, value);
        }

        private string _sourceProgram;
        public string SourceProgram
        {
            get => _sourceProgram;
            set => SetProperty(ref _sourceProgram, value);
        }

        private bool _stopWorkingAfterThisTask;
        public bool StopWorkingAfterThisTask
        {
            get => _stopWorkingAfterThisTask;
            set => SetProperty(ref _stopWorkingAfterThisTask, value);
        }
        
        private void OnStopWorkingNow(object parameter)
        {
            _getCurrentTaskThread?.Interrupt();

            _stateManager.StopWorkingNow();

            Status = "Stopped working";
        }


        private void TaskHasFinished()
        {
            if (StopWorkingAfterThisTask)
            {
                _stateManager.StopWorkingAfterThisTask();
            }
            else
            {
                GetCurrentTask();
            }
        }

        private void StoppedWorking()
        {
            
        }

        private void GetCurrentTask()
        {
            _getCurrentTaskThread?.Interrupt();

            _getCurrentTaskThread = new Thread(() =>
            {
                // SourceProgram = "No program for now...";
                SourceProgram = "";
                Status = "Getting task";
                do
                {
                    try
                    {
                        Thread.Sleep(500);
                    }
                    catch (ThreadInterruptedException e)
                    {
                        return;
                    }
                    
                    CurrentTask = _stateManager.GetCurrentTask();
                } while (CurrentTask == null);

                Status = "Working on task";
                // Get batch information, set appropriate fields

                // SourceProgram = CurrentTask.getSource();

                // Start a task timer?? 
            })
            {
                IsBackground = true
            };
            _getCurrentTaskThread.Start();
        }
    }
}