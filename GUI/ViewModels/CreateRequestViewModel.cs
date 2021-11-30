using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Client.Clients;
using Client.Models;
using Client.Services;
using GUI.Helpers;

namespace GUI.ViewModels
{
    public class CreateRequestViewModel : ViewModelBase
    {
        private BatchClient _batchClient;
        
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

        private string _sourceProgram;
        public string SourceProgram
        {
            get => _sourceProgram;
            set => SetProperty(ref _sourceProgram, value);
        }

        private string _inputFiles;
        public string InputFiles
        {
            get => _inputFiles;
            set => SetProperty(ref _inputFiles, value);
        }

        private string _replication;
        public string Replication
        {
            get => _replication;
            set => SetProperty(ref _replication, value);
        }
        
        public ICommand CreateRequestCommand => _createRequestCommand;
        private readonly DelegateCommand _createRequestCommand;


        private bool _batchUploaded;
        public bool BatchUploaded
        {
            get => _batchUploaded;
            set => SetProperty(ref _batchUploaded, value);
        }

        private bool _batchNotUploaded;
        public bool BatchNotUploaded
        {
            get => _batchNotUploaded;
            set => SetProperty(ref _batchNotUploaded, value);
        }

        private string _uploadStatus;
        public string UploadStatus
        {
            get => _uploadStatus;
            set => SetProperty(ref _uploadStatus, value);
        }
        
        public CreateRequestViewModel()
        {
            _batchClient = new BatchClient(HttpService.GetHttpService());

            _createRequestCommand = new DelegateCommand(OnCreateRequest);

            BatchUploaded = false;
            BatchNotUploaded = true;
        }

        public void OnCreateRequest(object parameter)
        {
            List<string> inputFiles = InputFiles.Split(';').ToList();

            List<UserFile> userFiles = new List<UserFile>();

            for (int i = 0; i < inputFiles.Count(); i++)
            {
                if (String.IsNullOrWhiteSpace(inputFiles[i]))
                {
                    continue;
                }
                UserFile userFile = new UserFile("inputFile" + i, GenerateStreamFromString(inputFiles[i]));
                userFiles.Add(userFile);
            }

            UserFile sourceFile = new UserFile("sourceFile", GenerateStreamFromString(SourceProgram));

            Batch batch = new Batch("0", sourceFile, SourceLanguage, userFiles);

            bool success = _batchClient.AddBatch(batch);

            if (success)
            {
                BatchUploaded = true;
                BatchNotUploaded = false;
                UploadStatus = "Upload successful";
            }
            else
            {
                UploadStatus = "Upload failed";
            }
        }
        
        
        private Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}