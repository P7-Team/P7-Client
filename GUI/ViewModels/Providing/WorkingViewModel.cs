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
            get => "Title: " + _batchTitle;
            set => SetProperty(ref _batchTitle, value);
        }

        private string _batchDescription;
        public string BatchDescription
        {
            get => "Description: " + _batchDescription;
            set => SetProperty(ref _batchDescription, value);
        }

        private string _batchUser;
        public string BatchUser
        {
            get => "User :" + _batchUser;
            set => SetProperty(ref _batchUser, value);
        }
        
        

        private Task _currentTask;
        public Task CurrentTask
        {
            get => _currentTask;
            set => SetProperty(ref _currentTask, value);
        }

        private bool _stopWorkingAfterThisTask;
        public bool StopWorkingAfterThisTask
        {
            get => _stopWorkingAfterThisTask;
            set => SetProperty(ref _stopWorkingAfterThisTask, value);
        }
        
        private void OnStopWorkingNow(object parameter)
        {
            if (_getCurrentTaskThread != null)
            {
                _getCurrentTaskThread.Abort();
            }
            _stateManager.StopWorkingNow();
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

        private void GetCurrentTask()
        {
            if (_getCurrentTaskThread != null)
            {
                _getCurrentTaskThread.Abort();
            }
            
            _getCurrentTaskThread = new Thread(() =>
            {
                do
                {
                    Thread.Sleep(1000);
                    CurrentTask = _stateManager.GetCurrentTask();
                } while (CurrentTask == null);
                
                // Get batch information, set appropriate fields
                
                // Start a task timer?? 
            });
            _getCurrentTaskThread.Start();
        }
    }
}