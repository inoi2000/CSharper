using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Wpf.Ui.Common.Interfaces;
using CSharper.Models;
using System.Collections.Generic;
namespace CSharper.ViewModels
{
    public partial class BooksViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private List<Book> _books;

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        public void OnNavigatedFrom()
        {
        }

        private void InitializeViewModel()
        {
            var books = new List<Book>();
            books.Add(new Book() { Name = "Книга1" });
            books.Add(new Book() { Name = "Книга2" });
            books.Add(new Book() { Name = "Книга3" });

            Books = books;
            _isInitialized = true;
        }

        

        [RelayCommand]
        private void OnSelectBook(string parameter)
        {
           
        
        }
    }
}
