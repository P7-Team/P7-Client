using System.Windows;
using System.Windows.Controls;
using GUI.ViewModels;

namespace GUI.Views
{
    public partial class CreateUser : Page
    {
        private readonly CreateUserViewModel _viewModel;
        public CreateUser()
        {
            _viewModel = new CreateUserViewModel();

            DataContext = _viewModel;
            InitializeComponent();
        }
        
        private void UsernameBoxChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.Username = ((TextBox)sender).Text;
        }
        
        private void PasswordBox1Changed(object sender, RoutedEventArgs e)
        {
            _viewModel.Password = ((PasswordBox)sender).SecurePassword;
        }
        
        private void PasswordBox2Changed(object sender, RoutedEventArgs e)
        {
            _viewModel.SecondPassword = ((PasswordBox)sender).SecurePassword;
        }

        private void GotoLoginPage(object sender, RoutedEventArgs e)
        {
            _viewModel.CreateUserCommand.Execute(sender);
            if (_viewModel.UserCreated)
            {
                NavigationService.Navigate(new Login());
            }
        }
    }
}