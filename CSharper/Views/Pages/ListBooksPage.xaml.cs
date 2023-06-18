using Wpf.Ui.Common.Interfaces;

using System.Windows.Controls;
using System.Windows;
using CSharper.Models;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Linq;

using CSharper.Services;
using System.IO;
using System.Threading;
using System;

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
                //Reading r = (x.Reading == Reading.Yes) ? Reading.No : Reading.Yes;
                //x.setReading(r); 
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

        //private async void OpenSelectedBook(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //     ((sender as ComboBox).SelectedItem as RelayCommand).Execute(null);
        //}
        

        private async void OpenSelectedBook(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DownloadProgresRing.Visibility = Visibility.Visible;
            BooksListBox.Visibility = Visibility.Collapsed;

            await ViewModel.DownloadSelectedBook();

            DownloadProgresRing.Visibility = Visibility.Collapsed;
            BooksListBox.Visibility = Visibility.Visible;

            //Application.Current.Windows.OfType<Views.Windows.MainWindow>().First()?
            //        .RootFrame.Navigate(new Views.Pages.PdfViewerPage(ViewModel.LocalPath));



            var mainWindow = Application.Current.Windows.OfType<Windows.MainWindow>().First();

            var readerWindow = Application.Current.Windows.OfType<Windows.PdfViewerWindow>().First();

            mainWindow.Visibility = Visibility.Collapsed;

            readerWindow.Open(ViewModel.LocalPath);
            readerWindow.ShowDialog();
            mainWindow.Visibility = Visibility.Visible;

            DownloadProgresRing.Visibility = Visibility.Collapsed;
            BooksListBox.Visibility = Visibility.Visible;

            //Application.Current.Windows.OfType<Views.Windows.MainWindow>().First()?
            //        .RootFrame.Navigate(new Views.Pages.PdfViewerPage(ViewModel.LocalPath));
        }

        private void SelectListBook(object sender, SelectionChangedEventArgs e)
        {
            //ViewModel.SelectCommands[ ((sender as ComboBox).SelectedItem as string)].Execute(null);

        }
    }
}