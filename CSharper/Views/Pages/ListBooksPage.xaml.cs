﻿using Wpf.Ui.Common.Interfaces;

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

        //private static RelayCommand<Book> readingClickCommand = new RelayCommand<Book>
        //   (x =>
        //   {
        //       if (x == null) return;
        //   });
        //public static RelayCommand<Book> ReadingClickCommand
        //{
        //    get { return readingClickCommand; }
        //}

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

        //private void SelectListBook(object sender, SelectionChangedEventArgs e)
        //{
        //    //ViewModel.SelectCommands[ ((sender as ComboBox).SelectedItem as string)].Execute(null);

        //}

        private async void SelectCurrentSubject(object sender, SelectionChangedEventArgs e)
        {
          
            await ViewModel.GetBooksOnFilter();
        }

        private async void ChangeFindName(object sender, TextChangedEventArgs e)
        {
            await ViewModel.GetBooksOnFilter();
        }
    }
}