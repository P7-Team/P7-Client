using System.Threading;
using System.Windows.Input;
using GUI.Helpers;

namespace GUI.ViewModels.Providing
{
    public class NotWorkingViewModel : ViewModelBase
    {
        // public ICommand StartWorkingCommand => _startWorkingCommand;
        // private readonly DelegateCommand _startWorkingCommand;
        
        // public NotWorkingViewModel() : base()
        // {
        //     _startWorkingCommand = new DelegateCommand(OnStartWorking);
        // }
        
        // private void OnStartWorking(object parameter)
        // {
        //     CurrentTask = null;
        //     _stateManager.StartWorking();
        //     do
        //     {
        //         Thread.Sleep(100);
        //         CurrentTask = _stateManager.GetCurrentTask();
        //     } while (CurrentTask == null);
        //     
        //     // TODO: We need to get batch information:
        //     // Title
        //     // Description
        //     // Number of tasks done
        //     // Number of points spent on the batch
        // }
    }
}