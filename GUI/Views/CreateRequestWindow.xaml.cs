using System.Windows;
using System.Windows.Controls;
using GUI.ViewModels;

namespace GUI.Views
{
    public partial class CreateRequestWindow : Window
    {
        private CreateRequestViewModel _viewModel;
        public CreateRequestWindow()
        {
            _viewModel = new CreateRequestViewModel();

            DataContext = _viewModel;
            InitializeComponent();
        }

        private void ReplicationChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.Replication = ((TextBlock)((ComboBox)sender).SelectedValue).Text;
        }

        private void SourceLanguageChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.SourceLanguage = ((TextBlock)((ComboBox)sender).SelectedValue).Text;
        }

        private void SourceVersionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.SourceVersion = ((TextBlock)((ComboBox)sender).SelectedValue).Text;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}