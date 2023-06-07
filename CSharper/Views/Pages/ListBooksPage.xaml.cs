using Wpf.Ui.Common.Interfaces;

using System.Windows.Controls;
using System.Windows;
using CSharper.Models;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;

namespace CSharper.Views.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class ListBooksPage : INavigableView<ViewModels.ListBooksViewModel>
    {
        private static RelayCommand<Book> readingClickCommand=new RelayCommand<Book>
            (x =>
            {
                if (x == null) return;
                Reading r = (x.Reading == Reading.Yes) ? Reading.No : Reading.Yes;
                x.setReading(r); 
               });
        public static RelayCommand<Book> ReadingClickCommand
        {
            get  {   return readingClickCommand;   }
        }

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
             ((sender as ComboBox).SelectedItem as RelayCommand).Execute(null);
        }
        

        private void OpenSelectedBook(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //ViewModel.Books.RemoveAt(0); //a удаление отражается
            
            var path = ((sender as ListBox).SelectedItem as Book).LocalLink;
            (Application.Current.Windows[0] as Views.Windows.MainWindow).RootNavigation.
                PageService.GetPage<BooksPage>()._NavigationFrame.Navigate(new Views.Pages.PdfViewerPage(path));


        }

        private void SelectListBook(object sender, SelectionChangedEventArgs e)
        {
            //ViewModel.SelectCommands[ ((sender as ComboBox).SelectedItem as string)].Execute(null);

        }
    }
}