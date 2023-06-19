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
using Wpf.Ui.Controls.Interfaces;
using System.Windows.Controls;
using System.Diagnostics;
using System.Threading;
using static System.Reflection.Metadata.BlobBuilder;

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

        [ObservableProperty]

        private IEnumerable<Book> _books;

        [ObservableProperty]
        private IEnumerable<Subject> _subjects;

        [ObservableProperty]
        private Book _selectedBook;

        public string LocalPath => SelectedBook?.LocalLink ?? string.Empty;

        private string _selectedOrderingType;
        public string SelectedOrderingType
        {
            get { return _selectedOrderingType; }
            set
            {
                _selectedOrderingType = value;
                OnPropertyChanged(nameof(SelectedOrderingType));
                SelectCommands[_selectedOrderingType].Execute(null);
            }
        }

        [ObservableProperty]
        private Dictionary<string,RelayCommand> _selectCommands;


        public RelayCommand SelectViewAllBookCommand => new RelayCommand(() => { FromDB("Книга"); });//, "Все"
        public RelayCommand SelectViewNoReadBookCommand => new RelayCommand(() => { FromDB("2"); });//, "Непрочитанные"
        public RelayCommand SelectViewMostExperienceCommand => new RelayCommand(() => { FromDB("3"); });//, "С наибольшим опытом"
        public RelayCommand SelectViewHighestComplexityCommand => new RelayCommand(() => { FromDB("3"); });//, "Самые сложноы"
        public RelayCommand SelectViewLowestComplexityCommand => new RelayCommand(() => { FromDB("3"); });//, "Самые легкие"



        public async void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();

            cts = new CancellationTokenSource();

            _subjectService = new SubjectService();
            Subjects = await _subjectService.GetAllSubjectsAcync();

            _bookService = new BookService();
            Books = await _bookService.GetAllBooksAsync();
        }

        public void OnNavigatedFrom()
        {
            cts.Cancel();
            _subjectService?.Dispose();
            _bookService?.Dispose();
        }


        private void InitializeViewModel()
        {
            //SelectCommands = new Dictionary<string,RelayCommand>();
            //SelectCommands.Add("Все", SelectViewAllBookCommand);
            //SelectCommands.Add("Непрочитанные", SelectViewNoReadBookCommand);
            //SelectCommands.Add("С наибольшим опытом", SelectViewMostExperienceCommand);
            //SelectCommands.Add("Сложные", SelectViewHighestComplexityCommand);
            //SelectCommands.Add("Легкие", SelectViewLowestComplexityCommand);

            _isInitialized = true;
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
            return await _bookService.DownloadBookAsync(SelectedBook.Id, progress, token);
        }

        public RelayCommand DownloadSelectedBookCommand => new RelayCommand(async () => { await DownloadSelectedBook(); });

        [RelayCommand]
        private async Task ReadBook()
        {
            if (AppConfig.IsСurrentUserDefault()) return;

            await _bookService.AccomplitBookAsync(AppConfig.User.Id, _selectedBook.Id);
        }

        public async Task FromDB(string t)
        {
            //TODO реализация функционала групировки книг
        }
    }

}
