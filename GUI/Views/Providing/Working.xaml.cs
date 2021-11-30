using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using GUI.ViewModels.Providing;

namespace GUI.Views.Providing
{
    public partial class Working : Page
    {
        private WorkingViewModel _viewModel;
        
        public Working()
        {
            _viewModel = new WorkingViewModel();

            DataContext = _viewModel;

            InitializeComponent();
        }

        private void StopWorkingClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NotWorking());
        }
    }
}