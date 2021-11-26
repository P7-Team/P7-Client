using System.Windows.Controls;
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
    }
}