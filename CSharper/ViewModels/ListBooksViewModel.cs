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
using CSharper.Services;
using System.Threading.Tasks;
using System.Threading;


namespace CSharper.ViewModels
{
    public partial class ListBooksViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;


        private static CancellationTokenSource cts = null;

        private SubjectService _subjectService { get; set; }
        private BookService _bookService { get; set; }


        [ObservableProperty]
        private User _currentUser;

        [ObservableProperty]
        private Subject _currentSubject;

        //public void SetCurrentSubject()
        //{

        //    AppConfig.Subject = _currentSubject;
          

        //}

        [ObservableProperty]
        private IEnumerable<Book> _books;

        [ObservableProperty]
        private IEnumerable<Subject> _subjects;

        [ObservableProperty]
        private Book _selectedBook;

       
        [ObservableProperty]
        private Dictionary<string, Complexity?> _selectCommands=new Dictionary<string, Complexity?>()
        {
            { "Все",    null},
            { "Средний",Complexity.medium},
            { "Сложный",Complexity.hard}, 
            { "Продвинутый",Complexity.hardcore},
            { "Легкий",Complexity.easy }
        };

        private string _selectedOrderingType;
        public string SelectedOrderingType
        {
            get { return _selectedOrderingType; }
            set
            {
                _selectedOrderingType = value;
                OnPropertyChanged(nameof(SelectedOrderingType));
                _complexity = _selectCommands[SelectedOrderingType].Value;
                GetBooksOnFilter();
              
            }
        }
        public string LocalPath => SelectedBook?.LocalLink ?? string.Empty;

        
        [ObservableProperty]
        private string _findName;

        [ObservableProperty] 
        private Complexity _complexity;

        public async void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();

 
            cts = new CancellationTokenSource();


             //CurrentSubject = AppConfig.Subject;

           // if (_bookService == null) _bookService = new BookService();
           // await GetBooksOnSubject();

             _subjectService = new SubjectService();
             Subjects = await _subjectService.GetAllSubjectsAcync();

             _bookService=new BookService();
            Books=await _bookService.GetAllBooksAsync();

            CurrentSubject = AppConfig.Subject;

        }

        public void OnNavigatedFrom()
        {
            cts.Cancel();
            _subjectService?.Dispose();
            _bookService?.Dispose();
        }


        private void InitializeViewModel()
        {

            _isInitialized = true;
        }

        public async Task GetBooksOnFilter()
        {
            if (_bookService == null) return;

             IEnumerable<Book> books=await _bookService.GetAllBooksAsync();

           // books.ToList().ForEach(book => { book.Subject.Equals(_currentSubject)})
            if (_currentSubject != null)
              //  books.ToList().RemoveAll(x => x.Subject != _currentSubject);
                books=books.Where(book => book.Subject.ToString().Equals(_currentSubject.ToString())==true).ToList();

            //authorsList = authorsList.Where(x => x.FirstName != "Bob").ToList();
            if (!String.IsNullOrEmpty(_findName))
                books=books.Where(book => book.Name.Contains(_findName)==true);
            
            if(_complexity!=null)
                books = books.Where(book => book.Complexity == _complexity);

            _books = books;
        
        }
        

        private double _downloadProgress;
        public double DownloadProgress { 
            get { return _downloadProgress; }
            set { _downloadProgress = value; OnPropertyChanged(); }
        }


        public async Task<bool> DownloadSelectedBook()
        {
            var progress = new Progress<double>();
            progress.ProgressChanged += (sender, current) =>
            {
                DownloadProgress = current;
                OnPropertyChanged(nameof(DownloadProgress));
            };

            CancellationToken token = cts.Token;

            //
            //TODO реализавать отдельное исполнение метода
            await ReadBook(); // но пока он здесь
            //
            //return true;
            return await _bookService.DownloadBookAsync(_selectedBook.Id, progress, token);
        }

        public RelayCommand DownloadSelectedBookCommand => new RelayCommand(async () => { await DownloadSelectedBook(); });

        [RelayCommand]
        private async Task ReadBook()
        {
            await _bookService.AccomplitBookAsync(AppConfig.User.Id, _selectedBook.Id);
        }

        //public async Task FromDB(string t)
        //{
        //    //TODO реализация функционала групировки книг
        //}
    }

}
