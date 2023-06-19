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

        public void SetCurrentSubject()
        {

           AppConfig.Subject = _currentSubject;
          

        }

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
                _complexityBook = null;
                _selectCommands.TryGetValue(SelectedOrderingType, out _complexityBook);

                
               
                GetBooksOnFilter();
              
            }
        }
        public string LocalPath => SelectedBook?.LocalLink ?? string.Empty;

        
        [ObservableProperty]
        private string _findName;

        [ObservableProperty] 
        private Complexity? _complexityBook;

        public async void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();

 
            cts = new CancellationTokenSource();

             _subjectService = new SubjectService();
             Subjects = await _subjectService.GetAllSubjectsAcync();

             _bookService=new BookService();
            Books=await _bookService.GetAllBooksAsync();

            CurrentSubject = AppConfig.Subject;

            _complexityBook = null;

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

            if (_currentSubject != null)
                books=books.Where(book => book.Subject.Id==_currentSubject.Id).ToList();

            if (!String.IsNullOrEmpty(_findName))
                books=books.Where(book => book.Name.Contains(_findName)==true);
            
            if(_complexityBook !=null)
                books = books.Where(book => book.Complexity == _complexityBook);

            Books = books;
        
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

            if (_selectedBook == null) return false;
            //
            //
            //TODO реализавать отдельное исполнение метода
            await ReadBook(); // но пока он здесь
            //
            
            return await _bookService.DownloadBookAsync(_selectedBook.Id, progress, token);
        }

        public RelayCommand DownloadSelectedBookCommand => new RelayCommand(async () => { await DownloadSelectedBook(); });

        [RelayCommand]
        private async Task ReadBook()
        {
            await _bookService.AccomplitBookAsync(AppConfig.User.Id, _selectedBook.Id);
        }

    }

}
