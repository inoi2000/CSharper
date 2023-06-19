using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Wpf.Ui.Common.Interfaces;
using CSharper.Models;
using System.Collections.Generic;
using System.Windows;
using CSharper.Services;
using System.Linq;

namespace CSharper.ViewModels
{
    public partial class HomeViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;
        private SubjectService _subjectService;

        public User User => AppConfig.User;

        [ObservableProperty]
        private IEnumerable<Subject> _subjects;

        [ObservableProperty]
        private Subject _currentSubject;

        public void SetCurrentSubject()
        {
          
            AppConfig.Subject = _currentSubject;
                       
        }


        private static RelayCommand<Subject> subjectClickCommand = new RelayCommand<Subject>
           (x => { AppConfig.Subject = x; });
        public static RelayCommand<Subject> SubjectClickCommand
        {
            get { return subjectClickCommand; }
        }


        public async void OnNavigatedTo()
        {
            if (!_isInitialized)
            {
                InitializeViewModel();
            }
            _subjectService = new SubjectService();
            Subjects = await _subjectService.GetAllSubjectsAcync();

            CurrentSubject = AppConfig.Subject;
        }

        
        public void OnNavigatedFrom()
        {
        }


        public HomeViewModel()
        {
            InitializeViewModel();
        }
        private  void InitializeViewModel()
        {
           
            _isInitialized = true;

        }

    }
}
