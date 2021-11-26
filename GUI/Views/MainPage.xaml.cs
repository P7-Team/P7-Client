using System.Windows;
using System.Windows.Controls;
using Client.Services;
using GUI.ViewModels;

namespace GUI.Views
{
    public partial class MainPage : Page
    {
        private readonly MainViewModel _viewModel;
        
        public MainPage()
        {
            _viewModel = new MainViewModel();

            DataContext = _viewModel;
            InitializeComponent();
        }

        private void CreateRequest(object sender, RoutedEventArgs e)
        {
            Window win = new CreateRequestWindow();
            win.Show();
        }
    }
}