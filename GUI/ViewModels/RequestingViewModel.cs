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
    public class RequestingViewModel : ViewModelBase
    {
        private readonly ClientStateManager _stateManager;
        private readonly BatchClient _batchClient;

        private List<BatchStatus> _myBatchStatusList;
        public List<BatchStatus> MyBatchStatusList
        {
            get => _myBatchStatusList;
            set => SetProperty(ref _myBatchStatusList, value);
        }
        
        public ICommand SaveResultsCommand => _saveResultsCommand;
        private readonly DelegateCommand _saveResultsCommand;

        public ICommand FetchBatchesCommand => _fetchBatchesCommand;
        private readonly DelegateCommand _fetchBatchesCommand;



        private bool _resultsAreReady;

        public bool ResultsAreReady
        {
            get => _resultsAreReady;
            set => SetProperty(ref _resultsAreReady, value);
        }

        public RequestingViewModel()
        {
            _stateManager = ClientStateManager.GetClientStateManager();
            _batchClient = new BatchClient(HttpService.GetHttpService());

            _stateManager.FetchedBatch += BatchesFetched;
            _stateManager.Run();

            MyBatchStatusList = new List<BatchStatus>();

            _saveResultsCommand = new DelegateCommand(OnSaveResults);
            _fetchBatchesCommand = new DelegateCommand(OnFetchCommands);

            ResultsAreReady = true;
        }

        private void OnSaveResults(object parameters)
        {
            MyBatchStatusList = _stateManager.GetBatchStatuses();
            
            var path = "C:/Users/aneso/Documents";
            _batchClient.GetResult(MyBatchStatusList, path);
        }

        private void OnFetchCommands(object parameters)
        {
            _stateManager.StartFetchingBatches();
        }

        private void BatchesFetched()
        {
            MyBatchStatusList = _stateManager.GetBatchStatuses();
        }
    }
}