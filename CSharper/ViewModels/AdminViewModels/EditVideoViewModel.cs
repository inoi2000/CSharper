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
    public partial class EditVideoViewModel : ObservableObject, INavigationAware
    {
        public IEnumerable<Complexity> Complexities => Enum.GetValues(typeof(Complexity)).Cast<Complexity>();
        private VideoService _videoService { get; set; }
        private SubjectService _subjectService { get; set; }


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditVideoCommand))]
        private string _name = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditVideoCommand))]
        private string? _description = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditVideoCommand))]
        private double _experience = 0D;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditVideoCommand))]
        private string _uri = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditVideoCommand))]
        private Complexity _complexity = 0;

        [ObservableProperty]
        private IEnumerable<Subject> _subjects;

        [ObservableProperty]
        private IEnumerable<Video> _videos;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditVideoCommand))]
        private Subject _selectedSubject;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditVideoCommand))]
        private Complexity _selectedComplexity;


        private Video _selectedVideo;
        public Video SelectedVideo
        {
            get
            {
                return _selectedVideo;
            }
            set
            {
                _selectedVideo = value;
                OnPropertyChanged(nameof(SelectedVideo));
                if (SelectedVideo is Video video)
                {
                    Name = video.Name;
                    Description = video.Description;
                    Experience = video.Experience;
                    Uri = video.Url?.ToString() ?? string.Empty;
                    SelectedComplexity = video.Complexity;
                    SelectedSubject = Subjects?.FirstOrDefault(s => s.Id == video.Subject.Id);
                }
            }
        }

        public async void OnNavigatedTo()
        {
            _subjectService = new SubjectService();
            _videoService = new VideoService();

            Subjects = await _subjectService.GetAllSubjectsAcync();
            Videos = await _videoService.GetAllVideosAsync();
        }

        public void OnNavigatedFrom()
        {
            _videoService?.Dispose();
            _subjectService?.Dispose();
        }

        [RelayCommand(CanExecute = nameof(CanEditVideo))]
        private async Task EditVideo()
        {
            var video = new Video()
            {
                Id = Guid.NewGuid(),
                Name = this.Name,
                Description = this.Description,
                Complexity = SelectedComplexity,
                Experience = Experience,
                Url = new Uri(Uri),
                Subject = SelectedSubject
            };

            await _videoService.EditVideo(video, SelectedVideo.Id);
            Videos = await _videoService.GetAllVideosAsync();
        }

        private bool CanEditVideo()
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                !string.IsNullOrWhiteSpace(Uri) &&
                SelectedVideo != null &&
               (SelectedVideo.Name != this.Name ||
                SelectedVideo.Description != this.Description ||
                SelectedVideo.Complexity != this.SelectedComplexity ||
                SelectedVideo.Experience != this.Experience ||
                SelectedVideo.Url?.ToString() != this.Uri ||
                SelectedVideo.Subject.Id != SelectedSubject?.Id);
        }
    }
}
