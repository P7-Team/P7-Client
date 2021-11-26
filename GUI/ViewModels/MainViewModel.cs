using System;
using System.Windows.Input;
using Client.Services;
using GUI.Helpers;

namespace GUI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        // Something about changing between login, create user, and main page

        private ClientStateManager _stateManager; 
        
        private readonly DelegateCommand _changeSomethingCommand;
        public ICommand ChangeSomethingCommand => _changeSomethingCommand;

        public MainViewModel()
        {
            _changeSomethingCommand = new DelegateCommand(OnChangeSomething);
            
            _stateManager = ClientStateManager.GetClientStateManager();
        }

        private void OnChangeSomething(object parameter)
        {
            Console.WriteLine("Her!");
        }
    }
}