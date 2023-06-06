using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Wpf.Ui.Common.Interfaces;
using CSharper.Models;
using System.Collections.Generic;
using CSharper.Infrastructure.Commands;
using System.Windows;

namespace CSharper.ViewModels
{
    public partial class ListBooksViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        private List<Book> booksFromDB;

        [ObservableProperty]
        private List<Book> _books;

        [ObservableProperty]
        private List<string> _bookThemes;

        [ObservableProperty]
        private Book _selectBook;

        [ObservableProperty]
        private List<ActionCommand> _selectCommands;

        public ActionCommand SelectViewAllBookCommand => new ActionCommand(x => { FromDB("Книга"); }, null, "Все");
        public ActionCommand SelectViewNewBookCommand => new ActionCommand(x => { FromDB("1"); }, null, "Новые");
        public ActionCommand SelectViewNoReadBookCommand => new ActionCommand(x => { FromDB("2"); }, null, "Непрочитанные");
        public ActionCommand SelectViewBestBookCommand => new ActionCommand(x => { FromDB("3"); }, null, "С высоким рейтингом");

        public ActionCommand OpenSelectedBookCommand2 => new ActionCommand(x => { MessageBox.Show("");  },null,"Открыть pdf-файл");
        public ActionCommand SetBallCommand= new ActionCommand(x => { MessageBox.Show(""); }, null, "Открыть pdf-файл");
        public ActionCommand LoadBookCommand = new ActionCommand(x => { MessageBox.Show(""); }, null, "Открыть pdf-файл");
        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        public void OnNavigatedFrom()
        {
        }

        public void FromDB(string t)
        { 
            Books= booksFromDB.FindAll(x=>x.Name.Contains(t)); 
        }

        public ListBooksViewModel()
        { 
          InitializeViewModel();
        }
        private void InitializeViewModel()
        {  
            booksFromDB = new List<Book>();
            booksFromDB.Add(new Book() { Name = "Книга1", LocalLink = "DownloadBooks/books.pdf" });
            booksFromDB.Add(new Book() { Name = "Книга2", LocalLink = "DownloadBooks/books.pdf" });
            booksFromDB.Add(new Book() { Name = "Книга3", LocalLink = "DownloadBooks/books.pdf" });
            booksFromDB.Add(new Book() { Name = "Книга1", LocalLink = "DownloadBooks/books.pdf" });
            booksFromDB.Add(new Book() { Name = "Книга2", LocalLink = "DownloadBooks/books.pdf" });
            booksFromDB.Add(new Book() { Name = "Книга3", LocalLink = "DownloadBooks/books.pdf" });
            booksFromDB.Add(new Book() { Name = "Книга1", LocalLink = "DownloadBooks/books.pdf" });
            booksFromDB.Add(new Book() { Name = "Книга2", LocalLink = "DownloadBooks/books.pdf" });
            booksFromDB.Add(new Book() { Name = "Книга3", LocalLink = "DownloadBooks/books.pdf" });
            
            Books = booksFromDB;
            
            var bookThemes = new List<string>();
            bookThemes.Add("Все");
            bookThemes.Add("Тема 1");
            bookThemes.Add("Тема 2");
            bookThemes.Add("Тема 3");
            bookThemes.Add("Тема 4");

            BookThemes = bookThemes;

            var selectCommands = new List<ActionCommand>();
            selectCommands.Add(SelectViewAllBookCommand );
            selectCommands.Add(SelectViewNewBookCommand );
            selectCommands.Add(SelectViewNoReadBookCommand);
            selectCommands.Add(SelectViewBestBookCommand);

            SelectCommands= selectCommands;
            
            _isInitialized = true;
        }

        

        [RelayCommand]
        private void OnSelectBook(string parameter)
        {
           
        
        }

        private void OpenSelectedBook()
        {
             MessageBox.Show("");
            ////var path = SelectBook.LocalLink;
            ////new Wpf.Ui.Controls.NavigationItem()
            ////{
            ////    Content = "Читалка",
            ////    PageTag = "pdfviewer",
            ////    PageType = typeof(Views.Pages.PdfViewerPage)
            ////};
        //    Views.Pages.BooksPage._NavigationFrame.Navigate(new Views.Pages.PdfViewerPage());

        }
    }
}
