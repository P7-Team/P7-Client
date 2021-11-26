using System;
using System.Windows;
using System.Windows.Controls;
using GUI.ViewModels;

namespace GUI.Views
{
    public partial class Login : Page
    {
        private readonly LoginViewModel _viewModel;
        
        public Login()
        {
            _viewModel = new LoginViewModel();
            
            DataContext = _viewModel;
            InitializeComponent();
        }
        
        private void UsernameBoxChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.Username = ((TextBox)sender).Text;
        }

        private void PasswordBoxChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.Password = ((PasswordBox)sender).SecurePassword;
        }

        private void GotoCreateUserPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CreateUser());
        }

        private void GotoMainPage(object sender, RoutedEventArgs e)
        {
            _viewModel.LoginCommand.Execute(sender);
            if (_viewModel.LoggedIn)
            {
                NavigationService.Navigate(new MainPage());
            }
        }
    }
}