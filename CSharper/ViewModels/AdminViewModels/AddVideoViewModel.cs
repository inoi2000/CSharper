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
    public partial class AddVideoViewModel : ObservableObject, INavigationAware
    {
        public IEnumerable<Complexity> Complexities => Enum.GetValues(typeof(Complexity)).Cast<Complexity>();
        private VideoService _videoService { get; set; }
        private SubjectService _subjectService { get; set; }


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddNewVideoCommand))]
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
        private IEnumerable<Video> _videos;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddNewVideoCommand))]
        private Subject _selectedSubject;

        [ObservableProperty]
        private Complexity _selectedComplexity;

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

        [RelayCommand(CanExecute = nameof(CanAddNewVideo))]
        private async Task AddNewVideo()
        {
            var video = new Video()
            {
                Id = Guid.NewGuid(),
                Name = this.Name,
                Description = this.Description,
                Complexity = SelectedComplexity,
                Url = new Uri(Uri),
            };

            if (await _videoService.AddVideo(video, SelectedSubject.Id))
            {
                Name = string.Empty;
                Description = string.Empty;
                Uri = string.Empty;

                Videos = await _videoService.GetAllVideosAsync();
            }
        }

        private bool CanAddNewVideo()
        {
            return !string.IsNullOrWhiteSpace(Name) && SelectedSubject != null;
        }
    }
}
