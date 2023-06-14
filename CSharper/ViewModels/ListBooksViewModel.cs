﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Wpf.Ui.Common.Interfaces;
using CSharper.Models;
using System.Collections.Generic;

using System.Windows;
using System.Linq;
using CSharper.Services;

namespace CSharper.ViewModels
{
    public partial class ListBooksViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        private BookService bookService;
        private SubjectService subjectService;
        [ObservableProperty]
        private User _currentUser;

        [ObservableProperty]
        private Subject _currentSubject;

        [ObservableProperty]
        private IEnumerable<Book> _books;

        [ObservableProperty]
        private IEnumerable<Subject> _subjects;

        [ObservableProperty]
        private Book _selectBook;

        [ObservableProperty]
        private Dictionary<string,RelayCommand> _selectCommands;

        public RelayCommand SelectViewAllBookCommand => new RelayCommand(() => {  });//, "Все"
        public RelayCommand SelectViewNewBookCommand => new RelayCommand(() => {  });//, "Новые"
        public RelayCommand SelectViewNoReadBookCommand => new RelayCommand(() => {  });//, "Непрочитанные"
        public RelayCommand SelectViewBestBookCommand => new RelayCommand(() => {  });//, "С высоким рейтингом"

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

        public ListBooksViewModel()
        {
            InitializeViewModel();
        }
        private async void InitializeViewModel()
        {
            bookService= new BookService();
            subjectService= new SubjectService();
            _currentUser = AppConfig.User;
            _currentSubject = AppConfig.Subject;
            _books =await bookService.GetAllBooksAsync();
            _subjects=await subjectService.GetAllSubjectsAcync();

            var selectCommands = new Dictionary<string,RelayCommand>();
            selectCommands.Add("Все",SelectViewAllBookCommand);
            selectCommands.Add("1",SelectViewNewBookCommand);
            selectCommands.Add("2",SelectViewNoReadBookCommand);
            selectCommands.Add("3",SelectViewBestBookCommand);

            _selectCommands = selectCommands;

            _isInitialized = true;
        }


   
    }

}
