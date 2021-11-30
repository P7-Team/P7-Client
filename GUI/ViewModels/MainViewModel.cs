using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Input;
using Client.Models;
using Client.Services;
using GUI.Helpers;

namespace GUI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ClientStateManager _stateManager;

        private List<Batch> _myBatchesList;
        public List<Batch> MyBatchesList
        {
            get => _myBatchesList;
            set => SetProperty(ref _myBatchesList, value);
        }
        

        public MainViewModel()
        {
            _stateManager = ClientStateManager.GetClientStateManager();
            _stateManager.Run();

            MyBatchesList = new List<Batch>();
        }
    }
}