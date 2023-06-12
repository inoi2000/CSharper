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
    public partial class EditLessonViewModel : ObservableObject, INavigationAware
    {
        public IEnumerable<Complexity> Complexities => Enum.GetValues(typeof(Complexity)).Cast<Complexity>();
        private LessonService _lessonService { get; set; }
        private SubjectService _subjectService { get; set; }


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditLessonCommand))]
        private string _name = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditLessonCommand))]
        private string? _description = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditLessonCommand))]
        private double _experience = 0D;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditLessonCommand))]
        private string _uri = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditLessonCommand))]
        private Complexity _complexity = 0;

        [ObservableProperty]
        private IEnumerable<Subject> _subjects;

        [ObservableProperty]
        private IEnumerable<Lesson> _lessons;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditLessonCommand))]
        private Subject _selectedSubject;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditLessonCommand))]
        private Complexity _selectedComplexity;


        private Lesson _selectedLesson;
        public Lesson SelectedLesson
        {
            get
            {
                return _selectedLesson;
            }
            set
            {
                _selectedLesson = value;
                OnPropertyChanged(nameof(SelectedLesson));
                if (SelectedLesson is Lesson lesson)
                {
                    Name = lesson.Name;
                    Description = lesson.Description;
                    Experience = lesson.Experience;
                    Uri = lesson.Url?.ToString() ?? string.Empty;
                    SelectedComplexity = lesson.Complexity;
                    SelectedSubject = Subjects?.FirstOrDefault(s => s.Id == lesson.Subject.Id);
                }
            }
        }

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

        [RelayCommand(CanExecute = nameof(CanEditLesson))]
        private async Task EditLesson()
        {
            var lesson = new Lesson()
            {
                Id = Guid.NewGuid(),
                Name = this.Name,
                Description = this.Description,
                Complexity = SelectedComplexity,
                Experience = Experience,
                Url = new Uri(Uri),
                Subject = SelectedSubject
            };

            await _lessonService.EditLesson(lesson, SelectedLesson.Id);
            Lessons = await _lessonService.GetAllLessonsAsync();
        }

        private bool CanEditLesson()
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                !string.IsNullOrWhiteSpace(Uri) &&
                SelectedLesson != null &&
               (SelectedLesson.Name != this.Name ||
                SelectedLesson.Description != this.Description ||
                SelectedLesson.Complexity != this.SelectedComplexity ||
                SelectedLesson.Experience != this.Experience ||
                SelectedLesson.Url?.ToString() != this.Uri ||
                SelectedLesson.Subject.Id != SelectedSubject?.Id);
        }
    }
}
