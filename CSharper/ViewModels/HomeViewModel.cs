using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Wpf.Ui.Common.Interfaces;
using CSharper.Models;
using System.Collections.Generic;
using System.Windows;

namespace CSharper.ViewModels
{
    public partial class HomeViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

         [ObservableProperty]
        private ObservableCollection<Subject> _subjects;

       
        //private Subject _currentSubject
        //{  get {
        //        return _stCurrentSubject; 
        //        }
        //   set
        //    {
        //        _stCurrentSubject= value;
        //    }
        //}

        public Subject CurrentSubject
        {
            get
            {
                return _stCurrentSubject;
            }
            set
            {
                SetProperty(ref _stCurrentSubject,value);
            }
        }


        [ObservableProperty]
        private static Subject _stCurrentSubject;
        

        private static RelayCommand<Subject> subjectClickCommand = new RelayCommand<Subject>
        (x =>
        {
            _stCurrentSubject = x;

           // MessageBox.Show(x.ToString());
            //SetCurrentSubject(x);
        }
        );
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

        [RelayCommand]
        private void OnCounterIncrement()
        {
          //  Counter++;
        }

        private void InitializeViewModel()
        {
            var subjects = new ObservableCollection<Subject>();
            subjects.Add(new Subject() { Name = "1",Complexity=Complexity.easy });
            subjects.Add(new Subject() { Name = "2", Complexity = Complexity.easy });
            subjects.Add(new Subject() { Name = "3", Complexity = Complexity.easy });
            subjects.Add(new Subject() { Name = "4", Complexity = Complexity.easy });
            subjects.Add(new Subject() { Name = "5", Complexity = Complexity.easy });
            Subjects=subjects;
            CurrentSubject= subjects[0];
           // CurrentSubject= _currentSubject;
        }

        public void SetCurrentSubject(Subject s)
        {
            CurrentSubject = s;

        }

    }
    
}
