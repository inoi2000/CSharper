using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Wpf.Ui.Common.Interfaces;
using CSharper.Models;
using System.Collections.Generic;
using CSharper.Infrastructure.Commands;

namespace CSharper.ViewModels
{
    public partial class BooksViewModel : ObservableObject, INavigationAware
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
        private List<ActionCommand> _selectCommands ;
 
        public ActionCommand SelectViewAllBookCommand => new ActionCommand(x => { FromDB("Книга"); },null,"Все");
        public ActionCommand SelectViewNewBookCommand => new ActionCommand(x => { FromDB("1"); }, null, "Новые");
        public ActionCommand SelectViewNoReadBookCommand => new ActionCommand(x => { FromDB("2"); }, null, "Непрочитанные");
        public ActionCommand SelectViewBestBookCommand => new ActionCommand(x => { FromDB("3"); }, null, "С высоким рейтингом");

 
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

        private void InitializeViewModel()
        {  
            booksFromDB = new List<Book>();
            booksFromDB.Add(new Book() { Name = "Книга1", LocalLink = "c:/books.png" });
            booksFromDB.Add(new Book() { Name = "Книга2", LocalLink = "c:/books.png" });
            booksFromDB.Add(new Book() { Name = "Книга3", LocalLink = "c:/books.png" });
            booksFromDB.Add(new Book() { Name = "Книга1", LocalLink = "c:/books.png" });
            booksFromDB.Add(new Book() { Name = "Книга2", LocalLink = "c:/books.png" });
            booksFromDB.Add(new Book() { Name = "Книга3", LocalLink = "c:/books.png" });
            booksFromDB.Add(new Book() { Name = "Книга1", LocalLink = "c:/books.png" });
            booksFromDB.Add(new Book() { Name = "Книга2", LocalLink = "c:/books.png" });
            booksFromDB.Add(new Book() { Name = "Книга3", LocalLink = "c:/books.png" });
            
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
    }
}
