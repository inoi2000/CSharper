using Wpf.Ui.Common.Interfaces;

using System.Windows.Controls;
using System.Windows;
using System.Linq;

namespace CSharper.Views.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class BooksPage : INavigableView<ViewModels.BooksViewModel>
    {
        public ViewModels.BooksViewModel ViewModel
        {
            get;
        }

        public BooksPage(ViewModels.BooksViewModel viewModel)
        {
             ViewModel = viewModel;

            InitializeComponent();
            //var mainWindow = Application.Current.Windows.OfType<Views.Windows.MainWindow>().First();
            //Application.Current.Windows.OfType<Views.Windows.MainWindow>().First().Navigate(typeof(Views.Pages.ListBooksPage));
            _NavigationFrame.Navigate(new ListBooksPage());
        }

    }
}