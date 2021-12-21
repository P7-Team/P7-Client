using System.Windows;
using GUI.ViewModels;

namespace GUI.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var viewModel = new RequestingViewModel();

            DataContext = viewModel;
            InitializeComponent();
        }
    }
}