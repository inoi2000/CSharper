using CommunityToolkit.Mvvm.ComponentModel;
using CSharper.Models;
using CSharper.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Common.Interfaces;

namespace CSharper.ViewModels
{
    public partial class VideosViewModel : ObservableObject, INavigationAware
    {
        private VideoService _videoService;
        private SubjectService _subjectService;

        [ObservableProperty]
        private IEnumerable<Video> _videos;

        [ObservableProperty]
        private Video selectedVideo;

        [ObservableProperty]
        private Subject selectedSubject;

        [ObservableProperty]
        private IEnumerable<Subject> _subjects;

        public async void OnNavigatedTo()
        {
            _subjectService = new SubjectService();
            _videoService = new VideoService();

            Subjects = await _subjectService.GetAllSubjectsAcync();

            if (SelectedSubject != null) { Videos = await _videoService.GetAllVideosAsync(SelectedSubject.Id); }
            else { Videos = await _videoService.GetAllVideosAsync(); }
        }

        public void OnNavigatedFrom()
        {
            _videoService.Dispose();
            _subjectService.Dispose();
        }

        public async Task UpdateVideosList()
        {
            if (SelectedSubject != null) Videos = await _videoService.GetAllVideosAsync(SelectedSubject.Id);
        }
    }
}
