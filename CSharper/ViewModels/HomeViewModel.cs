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
        private SubjectService subjectService;

        public User User => AppConfig.User;

        [ObservableProperty]
        private IEnumerable<Subject> _subjects;


        public Subject CurrentSubject
        {
            get
            {
                return AppConfig.Subject;
            }
            
        }


        private static RelayCommand<Subject> subjectClickCommand = new RelayCommand<Subject>
           (x => { AppConfig.Subject = x; });
        public static RelayCommand<Subject> SubjectClickCommand
        {
            get { return subjectClickCommand; }
        }


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


        public HomeViewModel()
        {
            InitializeViewModel();
        }
        private async void InitializeViewModel()
        {
            subjectService = new SubjectService();
            _subjects = await subjectService.GetAllSubjectsAcync();
           
            _isInitialized = true;

        }

    }
}
