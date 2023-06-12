using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSharper.Models;
using CSharper.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Common.Interfaces;

namespace CSharper.ViewModels.AdminViewModels
{
    public partial class EditAssignmentViewModel : ObservableObject, INavigationAware
    {
        public IEnumerable<Complexity> Complexities => Enum.GetValues(typeof(Complexity)).Cast<Complexity>();
        private AssignmentService _assignmentService { get; set; }
        private SubjectService _subjectService { get; set; }


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditAssignmentCommand))]
        private string _name = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditAssignmentCommand))]
        private string? _description = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditAssignmentCommand))]
        private double _experience = 0D;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditAssignmentCommand))]
        private string _uri = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditAssignmentCommand))]
        private Complexity _complexity = 0;

        [ObservableProperty]
        private IEnumerable<Subject> _subjects;

        [ObservableProperty]
        private IEnumerable<Assignment> _assignments;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditAssignmentCommand))]
        private Subject _selectedSubject;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditAssignmentCommand))]
        private Complexity _selectedComplexity;


        private Assignment _selectedAssignment;
        public Assignment SelectedAssignment
        {
            get
            {
                return _selectedAssignment;
            }
            set
            {
                _selectedAssignment = value;
                OnPropertyChanged(nameof(SelectedAssignment));
                if (SelectedAssignment is Assignment assignment)
                {
                    Name = assignment.Name;
                    Description = assignment.Description;
                    Experience = assignment.Experience;
                    Uri = assignment.Url?.ToString() ?? string.Empty;
                    SelectedComplexity = assignment.Complexity;
                    SelectedSubject = Subjects?.FirstOrDefault(s => s.Id == assignment.Subject.Id);
                }
            }
        }

        public async void OnNavigatedTo()
        {
            _subjectService = new SubjectService();
            _assignmentService = new AssignmentService();

            Subjects = await _subjectService.GetAllSubjectsAcync();
            Assignments = await _assignmentService.GetAllAssignmentsAsync();
        }

        public void OnNavigatedFrom()
        {
            _assignmentService?.Dispose();
            _subjectService?.Dispose();
        }

        [RelayCommand(CanExecute = nameof(CanEditAssignment))]
        private async Task EditAssignment()
        {
            var assignment = new Assignment()
            {
                Id = Guid.NewGuid(),
                Name = this.Name,
                Description = this.Description,
                Complexity = SelectedComplexity,
                Experience = Experience,
                Url = new Uri(Uri),
                Subject = SelectedSubject
            };

            await _assignmentService.EditAssignment(assignment, SelectedAssignment.Id);
            Assignments = await _assignmentService.GetAllAssignmentsAsync();
        }

        private bool CanEditAssignment()
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                !string.IsNullOrWhiteSpace(Uri) &&
                SelectedAssignment != null &&
               (SelectedAssignment.Name != this.Name ||
                SelectedAssignment.Description != this.Description ||
                SelectedAssignment.Complexity != this.SelectedComplexity ||
                SelectedAssignment.Experience != this.Experience ||
                SelectedAssignment.Url?.ToString() != this.Uri ||
                SelectedAssignment.Subject.Id != SelectedSubject?.Id);
        }
    }
}
