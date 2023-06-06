using Wpf.Ui.Common.Interfaces;
using CSharper.Infrastructure.Commands;
using System.Windows.Controls;
using System.Windows;
using CSharper.Models;

namespace CSharper.Views.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class ListBooksPage : INavigableView<ViewModels.ListBooksViewModel>
    {
    
        public ViewModels.ListBooksViewModel ViewModel
        {
            get;
        }

        public ListBooksPage()
        {
             ViewModel = new ViewModels.ListBooksViewModel();
             
             InitializeComponent();
        }

        private void SelectListBook(object sender, System.Windows.RoutedEventArgs e)
        {
             ((sender as ComboBox).SelectedItem as ActionCommand).Execute(null);
        }
        

        private void OpenSelectedBook(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var path = ((sender as ListBox).SelectedItem as Book).LocalLink;
            (Application.Current.Windows[0] as Views.Windows.MainWindow).RootNavigation.
                PageService.GetPage<BooksPage>()._NavigationFrame.Navigate(new Views.Pages.PdfViewerPage(path));


        }
    }
}