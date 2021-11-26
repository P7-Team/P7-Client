using System.Windows;
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
    }
}