using System.Security;
using Client.Clients;
using Client.Services;

namespace GUI.ViewModels
{
    public class UserLoginCreationViewModel : ViewModelBase
    {
        private string _username;
        private string _password;
        protected readonly UserClient UserClient;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        
        protected UserLoginCreationViewModel()
        {
            UserClient = new UserClient(HttpService.GetHttpService());
        }
    }
}