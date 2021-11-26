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