using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Input;
using Client.Clients;
using Client.Models;
using Client.Services;
using GUI.Helpers;

namespace GUI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ClientStateManager _stateManager;
        private BatchClient _batchClient;

        private List<BatchStatus> _myBatchesList;
        public List<BatchStatus> MyBatchesList
        {
            get => _myBatchesList;
            set => SetProperty(ref _myBatchesList, value);
        }
        
        public ICommand SaveResultsCommand => _saveResultsCommand;
        private readonly DelegateCommand _saveResultsCommand;

        private bool _resultsAreReady;

        public bool ResultsAreReady
        {
            get => _resultsAreReady;
            set => SetProperty(ref _resultsAreReady, value);
        }

        public MainViewModel()
        {
            _stateManager = ClientStateManager.GetClientStateManager();
            _batchClient = new BatchClient(HttpService.GetHttpService());

            _stateManager.FetchedBatch += FetchedBatch;
            _stateManager.Run();

            MyBatchesList = new List<BatchStatus>();

            _saveResultsCommand = new DelegateCommand(OnSaveResults);

            ResultsAreReady = true;
        }

        private void OnSaveResults(object parameters)
        {
            MyBatchesList = _stateManager.GetBatchStatuses();

            var path = "C:/Users/aneso/Documents";
            _batchClient.GetResult(MyBatchesList, path);
        }

        private void FetchedBatch()
        {
            MyBatchesList = _stateManager.GetBatchStatuses();
        }
    }
}