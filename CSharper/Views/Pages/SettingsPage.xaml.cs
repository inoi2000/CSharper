using CSharper.Services;
using CSharper.ViewModels;
using CSharper.Views.Windows;
using System.Linq;
using System.Windows;
using Wpf.Ui.Common.Interfaces;

namespace CSharper.Views.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : INavigableView<ViewModels.SettingsViewModel>
    {
        public ViewModels.SettingsViewModel ViewModel
        {
            get;
        }

        public SettingsPage(ViewModels.SettingsViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

        private void OpenAdminWindowBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var adminWindow = Application.Current.Windows.OfType<Views.Windows.AdminWindow>().First();
            var mainWindow = Application.Current.Windows.OfType<Views.Windows.MainWindow>().First();

            mainWindow.Visibility = Visibility.Collapsed;
            adminWindow.ShowDialog();
            mainWindow.Visibility = Visibility.Visible;
        }
    }
}