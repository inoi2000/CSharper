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
using System.Threading;
using System.Threading.Tasks;
using Wpf.Ui.Mvvm.Interfaces;

namespace CSharper.ViewModels
{
    public partial class ListLessonsViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        private static CancellationTokenSource cts = null;

        private LessonService _lessonService;
        private SubjectService _subjectService;
        
        public User CurrentUser { get { return AppConfig.User; } }

        [ObservableProperty]
        private Subject _currentSubject;

        public void SetCurrentSubject()
        {

            AppConfig.Subject = _currentSubject;

        }

        [ObservableProperty]
        private IEnumerable<Lesson> _lessons;

        [ObservableProperty]
        private IEnumerable<Subject> _subjects;

        [ObservableProperty]
        private Lesson _selectedLesson;

        
        [ObservableProperty]
        private Dictionary<string, Complexity?> _selectCommands = new Dictionary<string, Complexity?>()
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
                _complexityLesson = null;
                _selectCommands.TryGetValue(SelectedOrderingType, out _complexityLesson);

                GetLessonsOnFilter();
            }
        }
        public string LocalPath => _selectedLesson?.LocalLink ?? string.Empty;
       
        [ObservableProperty]
        private string _findName;
 
        [ObservableProperty]
        private Complexity? _complexityLesson;
        public async void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();

            cts = new CancellationTokenSource();

            _subjectService = new SubjectService();
            Subjects = await _subjectService.GetAllSubjectsAcync();

            _lessonService = new LessonService();
            await GetLessonsOnFilter();

            CurrentSubject = AppConfig.Subject;

        }
      
        public void OnNavigatedFrom()
        {
            cts.Cancel();
            _subjectService?.Dispose();
            _lessonService?.Dispose();
        }

        public ListLessonsViewModel()
        {
            InitializeViewModel();
        }
        private async void InitializeViewModel()
        {
          
            _isInitialized = true;
        }

        public async Task GetLessonsOnFilter()
        {
            if (_lessonService == null) return;

            IEnumerable<Lesson> lessons = await _lessonService.GetAllLessonsAsync();

             if (_currentSubject != null)
                lessons = lessons.Where(lesson => lesson.Subject.Id==_currentSubject.Id).ToList();

            
            if (!String.IsNullOrEmpty(_findName))
                lessons = lessons.Where(lesson => lesson.Name.Contains(_findName) == true);

            if (_complexityLesson != null)
                lessons = lessons.Where(lesson => lesson.Complexity == _complexityLesson);

           Lessons = lessons;

        }
        private double _downloadProgress;
        public double DownloadProgress
        {
            get { return _downloadProgress; }
            set { _downloadProgress = value; OnPropertyChanged(); }
        }


        public async Task<bool> DownloadSelectedLesson()
        {
            var progress = new Progress<double>();
            progress.ProgressChanged += (sender, current) =>
            {
                DownloadProgress = current;
                OnPropertyChanged(nameof(DownloadProgress));
            };

            CancellationToken token = cts.Token;
            if (_selectedLesson == null) return false;
            //
            //TODO реализавать отдельное исполнение метода
            await ReadLesson(); // но пока он здесь
            
            return await _lessonService.DownloadLessonAsync(_selectedLesson.Id, progress, token);
        }

        public RelayCommand DownloadSelectedLessonCommand => new RelayCommand(async () => { await DownloadSelectedLesson(); });

        [RelayCommand]
        private async Task ReadLesson()
        {
            await _lessonService.AccomplitLessonAsync(AppConfig.User.Id, _selectedLesson.Id);
        }


       
    }

}
