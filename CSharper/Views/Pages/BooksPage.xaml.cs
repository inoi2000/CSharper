using Wpf.Ui.Common.Interfaces;
using CSharper.Infrastructure.Commands;
using System.Windows.Controls;

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
        }

        private void SelectListBook(object sender, System.Windows.RoutedEventArgs e)
        {
            ((sender as ComboBox).SelectedItem as ActionCommand).Execute(null);
        }
    }
}