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
    public partial class EditSubjectViewModel : ObservableObject, INavigationAware
    {
        public IEnumerable<Complexity> Complexities => Enum.GetValues(typeof(Complexity)).Cast<Complexity>();
        private SubjectService _subjectService { get; set; }


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditSubjectCommand))]
        private string _name = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditSubjectCommand))]
        private string? _description = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditSubjectCommand))]
        private Complexity _complexity = 0;

        [ObservableProperty]
        private IEnumerable<Subject> _subjects;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditSubjectCommand))]
        private Complexity _selectedComplexity;


        private Subject _selectedSubject;
        public Subject SelectedSubject
        {
            get
            {
                return _selectedSubject;
            }
            set
            {
                _selectedSubject = value;
                OnPropertyChanged(nameof(SelectedSubject));
                if (SelectedSubject is Subject subject)
                {
                    Name = subject.Name;
                    Description = subject.Description;
                    SelectedComplexity = subject.Complexity;
                }
            }
        }

        public async void OnNavigatedTo()
        {
            _subjectService = new SubjectService();

            Subjects = await _subjectService.GetAllSubjectsAcync();
        }

        public void OnNavigatedFrom()
        {
            _subjectService?.Dispose();
        }

        [RelayCommand(CanExecute = nameof(CanEditSubject))]
        private async Task EditSubject()
        {
            var subject = new Subject()
            {
                Id = Guid.NewGuid(),
                Name = this.Name,
                Description = this.Description,
                Complexity = SelectedComplexity,
            };

            await _subjectService.EditSubject(subject, SelectedSubject.Id);
            Subjects = await _subjectService.GetAllSubjectsAcync();
        }

        private bool CanEditSubject()
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                SelectedSubject != null &&
               (SelectedSubject.Name != this.Name ||
                SelectedSubject.Description != this.Description ||
                SelectedSubject.Complexity != this.SelectedComplexity);
        }
    }
}
