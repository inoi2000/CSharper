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
    public partial class AddAssignmentViewModel : ObservableObject, INavigationAware
    {
        public IEnumerable<Complexity> Complexities => Enum.GetValues(typeof(Complexity)).Cast<Complexity>();
        private AssignmentService _assignmentService { get; set; }
        private SubjectService _subjectService { get; set; }


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddNewAssignmentCommand))]
        private string _name = string.Empty;

        [ObservableProperty]
        private string? _description = string.Empty;

        [ObservableProperty]
        private double _experience = 0D;

        [ObservableProperty]
        private string _uri = string.Empty;

        [ObservableProperty]
        private Complexity _complexity = 0;

        [ObservableProperty]
        private IEnumerable<Subject> _subjects;

        [ObservableProperty]
        private IEnumerable<Assignment> _assignments;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddNewAssignmentCommand))]
        private Subject _selectedSubject;

        [ObservableProperty]
        private Complexity _selectedComplexity;

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

        [RelayCommand(CanExecute = nameof(CanAddNewAssignment))]
        private async Task AddNewAssignment()
        {
            var assignment = new Assignment()
            {
                Id = Guid.NewGuid(),
                Name = this.Name,
                Description = this.Description,
                Complexity = SelectedComplexity,
                Url = new Uri(Uri),
            };

            if (await _assignmentService.AddAssignment(assignment, SelectedSubject.Id))
            {
                Name = string.Empty;
                Description = string.Empty;
                Uri = string.Empty;

                Assignments = await _assignmentService.GetAllAssignmentsAsync();
            }
        }

        private bool CanAddNewAssignment()
        {
            return !string.IsNullOrWhiteSpace(Name) && SelectedSubject != null;
        }
    }
}
