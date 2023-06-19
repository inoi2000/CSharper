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

        private SubjectService _subjectService { get; set; }
        private AssignmentService _assignmentService { get; set; }


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

        public string LocalPath => SelectedAssignment?.LocalLink ?? string.Empty;

        public async void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();

            cts = new CancellationTokenSource();

            _subjectService = new SubjectService();
            Subjects = await _subjectService.GetAllSubjectsAcync();

            _assignmentService = new AssignmentService();
            Assignments = await _assignmentService.GetAllAssignmentsAsync();
        }

        public void OnNavigatedFrom()
        {
            cts.Cancel();
            _subjectService?.Dispose();
            _assignmentService?.Dispose();
        }


        private void InitializeViewModel()
        {
            _isInitialized = true;
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

            //
            //TODO реализавать отдельное исполнение метода
            await ReadAssignment(); // но пока он здесь
            
            return await _assignmentService.DownloadAssignmentAsync(SelectedAssignment.Id, progress, token);
        }

        public RelayCommand DownloadSelectedBookCommand => new RelayCommand(async () => { await DownloadSelectedAssignment(); });

        [RelayCommand]
        private async Task ReadAssignment()
        {
            if (AppConfig.User.Login == "Неавторизованный пользователь") return;

            await _assignmentService.AccomplitAssignmentAsync(AppConfig.User.Id, _selectedAssignment.Id);
        }
    }

}
