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
           
            _NavigationFrame.Navigate(new ListBooksPage()); ; ;
            ;
        }

    }
}