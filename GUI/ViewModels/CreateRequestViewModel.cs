using Client.Clients;
using Client.Services;

namespace GUI.ViewModels
{
    public class CreateRequestViewModel : ViewModelBase
    {
        private BatchClient _batchClient;
        
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

        private string _sourceLanguage;
        public string SourceLanguage
        {
            get => _sourceLanguage;
            set => SetProperty(ref _sourceLanguage, value);
        }

        private string _sourceVersion;
        public string SourceVersion
        {
            get => _sourceVersion;
            set => SetProperty(ref _sourceVersion, value);
        }
        
        // List of source files
        
        // Arguments and which is main
        
        // Input files
        
        // Input files encoding
        
        
        // Replication
        
        // Link to result (Should probably be set when saving the result
        

        public CreateRequestViewModel()
        {
            _batchClient = new BatchClient(HttpService.GetHttpService());
        }
    }
}