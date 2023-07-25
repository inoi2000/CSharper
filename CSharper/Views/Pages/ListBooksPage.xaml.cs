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
using System.Windows.Media;
using CSharper.Helpers.Extensions;

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

  
        private async void OpenSelectedBook(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DownloadProgresRing.Visibility = Visibility.Visible;
            BooksListBox.Visibility = Visibility.Collapsed;

            await ViewModel.DownloadSelectedBook();

            DownloadProgresRing.Visibility = Visibility.Collapsed;
            BooksListBox.Visibility = Visibility.Visible;

            var mainWindow = Application.Current.Windows.OfType<Windows.MainWindow>().First();
            var readerWindow = Application.Current.Windows.OfType<Windows.PdfViewerWindow>().First();

            mainWindow.Visibility = Visibility.Collapsed;

            readerWindow.Open(ViewModel.SelectedBook);
            readerWindow.SetPdfReadingService(ViewModel._bookService);
            readerWindow.ShowDialog();

            mainWindow.Visibility = Visibility.Visible;

            DownloadProgresRing.Visibility = Visibility.Collapsed;
            BooksListBox.Visibility = Visibility.Visible;
        }

        private async void SelectCurrentSubject(object sender, SelectionChangedEventArgs e)
        {
            await ViewModel.DebounceFilter();
        }

        private async void ChangeFindName(object sender, TextChangedEventArgs e)
        {
            await ViewModel.DebounceFilter();
        }

        private async void BooksListBox_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer scrollViewer = BooksListBox.GetScrollViewer();
            if (scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight)
            {
                ViewModel.LoadNewBook();
                await ViewModel.DebounceFilter();
            }
        }
    }
}