using System.Windows;
using System.Windows.Controls;

namespace GUI.Views.Providing
{
    public partial class NotWorking : Page
    {
        public NotWorking()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Working());
        }
    }
}