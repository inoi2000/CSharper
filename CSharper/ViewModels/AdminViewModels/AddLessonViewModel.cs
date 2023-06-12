using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSharper.Models;
using CSharper.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Wpf.Ui.Common.Interfaces;

namespace CSharper.ViewModels.AdminViewModels
{
    public partial class AddLessonViewModel : ObservableObject, INavigationAware
    {
        public IEnumerable<Complexity> Complexities => Enum.GetValues(typeof(Complexity)).Cast<Complexity>();
        private LessonService _lessonService { get; set; }
        private SubjectService _subjectService { get; set; }


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddNewLessonCommand))]
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
        private IEnumerable<Lesson> _lessons;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddNewLessonCommand))]
        private Subject _selectedSubject;

        [ObservableProperty]
        private Complexity _selectedComplexity;

        public async void OnNavigatedTo()
        {
            _subjectService = new SubjectService();
            _lessonService = new LessonService();

            Subjects = await _subjectService.GetAllSubjectsAcync();
            Lessons = await _lessonService.GetAllLessonsAsync();
        }

        public void OnNavigatedFrom()
        {
            _lessonService?.Dispose();
            _subjectService?.Dispose();
        }

        [RelayCommand(CanExecute = nameof(CanAddNewLesson))]
        private async Task AddNewLesson()
        {
            var lesson = new Lesson()
            {
                Id = Guid.NewGuid(),
                Name = this.Name,
                Description = this.Description,
                Complexity = SelectedComplexity,
                Experience = Experience,
                Url = new Uri(Uri),
            };
            
            if(await _lessonService.AddLesson(lesson, SelectedSubject.Id))
            {
                Name = string.Empty;
                Description = string.Empty;
                Uri = string.Empty;

                Lessons = await _lessonService.GetAllLessonsAsync();
            }
        }

        private bool CanAddNewLesson()
        {
            return !string.IsNullOrWhiteSpace(Name) && SelectedSubject != null;
        }
    }
}
