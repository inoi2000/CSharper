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
    public partial class AddSubjectViewModel : ObservableObject, INavigationAware
    {
        public IEnumerable<Complexity> Complexities => Enum.GetValues(typeof(Complexity)).Cast<Complexity>();
        private SubjectService _subjectService { get; set; }


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddNewSubjectCommand))]
        private string _name = string.Empty;

        [ObservableProperty]
        private string? _description = string.Empty;

        [ObservableProperty]
        private Complexity _selectedComplexity;

        [ObservableProperty]
        private IEnumerable<Subject> _subjects;

        public async void OnNavigatedTo()
        {
            _subjectService = new SubjectService();
            Subjects = await _subjectService.GetAllSubjectsAcync();
        }

        public void OnNavigatedFrom()
        {
            _subjectService?.Dispose();
        }

        [RelayCommand(CanExecute = nameof(CanAddNewSubject))]
        private async Task AddNewSubject()
        {
            var subject = new Subject()
            {
                Id = Guid.NewGuid(),
                Name = this.Name,
                Description = this.Description,
                Complexity = SelectedComplexity
            };

            if(await _subjectService.AddSubject(subject))
            {
                Name = string.Empty;
                Description = string.Empty;
            }
        }

        private bool CanAddNewSubject()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }
    }
}
