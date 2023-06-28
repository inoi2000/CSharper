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
    public partial class ListAssignmentsViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;
        private static CancellationTokenSource cts = null;
        private DebounceDispatcher _debounceDispatcher;

        private SubjectService _subjectService { get; set; }
        public AssignmentService _assignmentService { get; set; }


        [ObservableProperty]
        private User _currentUser;

        [ObservableProperty]

        private Subject _currentSubject;

        [ObservableProperty]
        private IEnumerable<Assignment> _assignments;

        [ObservableProperty]
        private IEnumerable<Subject> _subjects;

        [ObservableProperty]
        private Assignment _selectedAssignment;

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
                _complexityAssignment = null;
                _selectCommands.TryGetValue(SelectedOrderingType, out _complexityAssignment);

                GetAssignmentsOnFilter();
            }
        }

        public string LocalPath => SelectedAssignment?.LocalLink ?? string.Empty;

        [ObservableProperty]
        private string _findName;

        [ObservableProperty]
        private Complexity? _complexityAssignment;

        public async void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();

            cts = new CancellationTokenSource();

            _subjectService = new SubjectService();
            Subjects = await _subjectService.GetAllSubjectsAcync();

            _assignmentService = new AssignmentService();
            //Assignments = await _assignmentService.GetAllAssignmentsAsync();

            await GetAssignmentsOnFilter();

            CurrentSubject = AppConfig.Subject;

            _complexityAssignment = null;
        }

        public void OnNavigatedFrom()
        {
            cts.Cancel();
            //_subjectService?.Dispose();
            //_assignmentService?.Dispose();
        }


        private void InitializeViewModel()
        {
            _debounceDispatcher = new DebounceDispatcher();
            _isInitialized = true;
        }

        public async Task DebounceFilter()
        {
            await _debounceDispatcher.Debounce(TimeSpan.FromMilliseconds(500), () => GetAssignmentsOnFilter());
        }

        private async Task GetAssignmentsOnFilter()
        {
            if (_assignmentService == null) return;

            IEnumerable<Assignment> assignments = await _assignmentService.GetAllAssignmentsAsync();

            if (_currentSubject != null)
                assignments = assignments.Where(assignment => assignment.Subject.Id == _currentSubject.Id).ToList();

            if (!String.IsNullOrEmpty(_findName))
                assignments = assignments.Where(assignment => assignment.Name.Contains(_findName) == true);

            if (_complexityAssignment != null)
                assignments = assignments.Where(assignment => assignment.Complexity == _complexityAssignment);

            Assignments = assignments;

        }

        private double _downloadProgress;
        public double DownloadProgress 
        { 
            get { return _downloadProgress; }
            set { _downloadProgress = value; OnPropertyChanged(); }
        }

        public async Task<bool> DownloadSelectedAssignment()
        {
            var progress = new Progress<double>();
            progress.ProgressChanged += (sender, current) =>
            {
                DownloadProgress = current;
                OnPropertyChanged(nameof(DownloadProgress));
            };

            CancellationToken token = cts.Token;
            
            return await _assignmentService.DownloadAssignmentAsync(SelectedAssignment.Id, progress, token);
        }

        public RelayCommand DownloadSelectedBookCommand => new RelayCommand(async () => { await DownloadSelectedAssignment(); });

        [RelayCommand]
        private async Task ReadAssignment()
        {
            if (AppConfig.IsСurrentUserDefault()) return;

            await _assignmentService.AccomplitAsync(AppConfig.User.Id, _selectedAssignment.Id);
        }
    }

}
