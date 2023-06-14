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

namespace CSharper.ViewModels
{
    public partial class ListLessonsViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        private LessonService lessonService;
        private SubjectService subjectService;
        [ObservableProperty]
        private User _currentUser;

        [ObservableProperty]
        private Subject _currentSubject;

        [ObservableProperty]
        private IEnumerable<Lesson> _lessons;

        [ObservableProperty]
        private IEnumerable<Subject> _subjects;

        [ObservableProperty]
        private Lesson _selectLesson;

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

        public ListLessonsViewModel()
        {
            InitializeViewModel();
        }
        private async void InitializeViewModel()
        {
            lessonService= new LessonService();
            subjectService= new SubjectService();
            CurrentUser = AppConfig.User;
            CurrentSubject = AppConfig.Subject;
            _lessons = await lessonService.GetAllLessonsAsync();
            _subjects=await subjectService.GetAllSubjectsAcync();
            _selectLesson = null;
            _isInitialized = true;
        }


   
    }

}
