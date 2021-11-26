using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Client.Services;
using GUI.Helpers;

namespace GUI.ViewModels
{
    public class LoginViewModel : UserLoginCreationViewModel
    {
        private readonly DelegateCommand _loginCommand;
        public ICommand LoginCommand => _loginCommand;

        public bool LoggedIn = false;

        private string _loginStatus = "";
        public string LoginStatus
        {
            get => _loginStatus;
            set
            {
                SetProperty(ref _loginStatus, value);
                OnPropertyChanged(nameof(LoginStatus));
            }
        }

        public LoginViewModel()
        {
            _loginCommand = new DelegateCommand(OnLogin);

            LoginStatus = "Nothing at the moment";
        }

        private void OnLogin(object parameter)
        {
            bool emptyUsername = String.IsNullOrEmpty(Username);
            bool emptyPassword = Password == null || String.IsNullOrEmpty(Password.ToString());
            
            if (emptyUsername)
            {
                LoginStatus = LoginStatus.Concat("Username was empty. ").ToString();
            }
            
            if (emptyPassword)
            {
                LoginStatus = LoginStatus.Concat("Password was empty. ").ToString();
            }
            
            if (emptyUsername || emptyPassword)
            {
                return;
            }
            
            try
            {
                HttpService.GetHttpService().SetToken(UserClient.LoginUser(Username, Password.ToString()));
                LoggedIn = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                LoginStatus = "Could not log in";
            }

            LoggedIn = true;
        }
    }
}