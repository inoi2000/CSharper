using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Wpf.Ui.Common.Interfaces;
using CSharper.Models;
using System.Collections.Generic;

using System.Windows;
using System.Linq;

namespace CSharper.ViewModels
{
    public partial class ListBooksViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        private ObservableCollection<Book> BooksFromDB;

        [ObservableProperty]
        private User _currentUser;

        [ObservableProperty]
        private ObservableCollection<Book> _books;

        [ObservableProperty]
        private List<string> _bookThemes;

        [ObservableProperty]
        private Book _selectBook;

        [ObservableProperty]
        private Dictionary<string,RelayCommand> _selectCommands;

        public RelayCommand SelectViewAllBookCommand => new RelayCommand(() => { FromDB("Книга"); });//, "Все"
        public RelayCommand SelectViewNewBookCommand => new RelayCommand(() => { FromDB("1"); });//, "Новые"
        public RelayCommand SelectViewNoReadBookCommand => new RelayCommand(() => { FromDB("2"); });//, "Непрочитанные"
        public RelayCommand SelectViewBestBookCommand => new RelayCommand(() => { FromDB("3"); });//, "С высоким рейтингом"

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
            {
                InitializeViewModel();
            }
        }
      
        public void OnNavigatedFrom()
        {
        }

        public void FromDB(string t)
        {
            //Books.Clear();
            //BooksFromDB.ToList().ForEach(x => Books.Add(x));

            //BooksFromDB.ToList().FindAll(x => !x.Name.Contains(t)).ForEach(x=>Books.Remove(x));
        }

        public ListBooksViewModel()
        {
            InitializeViewModel();
        }
        private void InitializeViewModel()
        {
            BooksFromDB = new ObservableCollection<Book>();
            BooksFromDB.Add(new Book() { Name = "Книга1", LocalLink = "DownloadBooks/books.pdf", Complexity = Complexity.easy });
            BooksFromDB.Add(new Book() { Name = "Книга2", LocalLink = "DownloadBooks/books.pdf", Complexity = Complexity.easy });
            BooksFromDB.Add(new Book() { Name = "Книга3", LocalLink = "DownloadBooks/books.pdf", Complexity = Complexity.easy });
            BooksFromDB.Add(new Book() { Name = "Книга1", LocalLink = "DownloadBooks/books.pdf", Complexity = Complexity.easy });
            BooksFromDB.Add(new Book() { Name = "Книга2", LocalLink = "DownloadBooks/books.pdf", Complexity = Complexity.easy });
            BooksFromDB.Add(new Book() { Name = "Книга3", LocalLink = "DownloadBooks/books.pdf", Complexity = Complexity.easy });
            BooksFromDB.Add(new Book() { Name = "Книга1", LocalLink = "DownloadBooks/books.pdf", Complexity = Complexity.easy });
            BooksFromDB.Add(new Book() { Name = "Книга2", LocalLink = "DownloadBooks/books.pdf", Complexity = Complexity.easy });
            BooksFromDB.Add(new Book() { Name = "Книга3", LocalLink = "DownloadBooks/books.pdf", Complexity = Complexity.easy });

            Books = BooksFromDB;
            //Books = new ObservableCollection<Book>();
            //BooksFromDB.ToList().ForEach(x=>Books.Add(x));

            CurrentUser = new User() { Id = 1 };

            Books[0].Users.Add(CurrentUser);
            Books[2].Users.Add(CurrentUser);
            Books[3].Users.Add(CurrentUser);
            Books[0].SetCurrentUser(CurrentUser);

            var bookThemes = new List<string>();
            bookThemes.Add("Все");
            bookThemes.Add("Тема 1");
            bookThemes.Add("Тема 2");
            bookThemes.Add("Тема 3");
            bookThemes.Add("Тема 4");

            BookThemes = bookThemes;

            //var selectCommands = new Dictionary<string,RelayCommand>();
            //selectCommands.Add("Все",SelectViewAllBookCommand);
            //selectCommands.Add("1",SelectViewNewBookCommand);
            //selectCommands.Add("2",SelectViewNoReadBookCommand);
            //selectCommands.Add("3",SelectViewBestBookCommand);

            //SelectCommands = selectCommands;
            _isInitialized = true;
        }


   
    }

}
