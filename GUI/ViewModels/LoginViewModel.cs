using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Client.Services;
using GUI.Helpers;
using Microsoft.VisualBasic;

namespace GUI.ViewModels
{
    public class LoginViewModel : UserLoginCreationViewModel
    {
        public ICommand LoginCommand => _loginCommand;
        private readonly DelegateCommand _loginCommand;

        public bool LoggedIn = false;

        private string _loginStatus;
        public string LoginStatus
        {
            get => _loginStatus;
            set => SetProperty(ref _loginStatus, value);
        }

        public LoginViewModel()
        {
            _loginCommand = new DelegateCommand(OnLogin);
        }

        private void OnLogin(object parameter)
        {
            bool emptyUsername = String.IsNullOrEmpty(Username);
            bool emptyPassword = String.IsNullOrEmpty(Password); 

            LoginStatus = "";
            
            if (emptyUsername)
            {
                LoginStatus += "Username was empty. ";
            }
            
            if (emptyPassword)
            {
                LoginStatus += "Password was empty. ";
            }
            
            if (emptyUsername || emptyPassword)
            {
                return;
            }

            string token = UserClient.LoginUser(Username, Password);

            if (String.IsNullOrEmpty(token))
            {
                LoginStatus = "Could not log in";
            }
            else
            {
                try
                {
                    var bytesArray = Encoding.UTF8.GetBytes(token);
                    var base64String = Convert.ToBase64String(bytesArray);
                    // HttpService.GetHttpService().SetToken(base64String);
                    LoggedIn = true;
                }
                catch
                {
                    // Something
                }
            }
        }
    }
}