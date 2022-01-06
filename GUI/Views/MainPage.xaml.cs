using System.Windows;
using System.Windows.Controls;
using Client.Services;
using GUI.ViewModels;

namespace GUI.Views
{
    public partial class MainPage : Page
    {
        private readonly RequestingViewModel _viewModel;
        
        public MainPage()
        {
            _viewModel = new RequestingViewModel();

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