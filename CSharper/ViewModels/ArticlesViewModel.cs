using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Common.Interfaces;
using CSharper.Models;
using System.Collections.ObjectModel;
using CSharper.Services;

namespace CSharper.ViewModels
{
    public partial class ArticlesViewModel : ObservableObject, INavigationAware
    {
        private ArticleService _articleService;
        private SubjectService _subjectService;

        [ObservableProperty]
        private IEnumerable<Article> _articles;

        [ObservableProperty]
        private Article selectedArticle;

        [ObservableProperty]
        private Subject selectedSubject;

        [ObservableProperty]
        private IEnumerable<Subject> _subjects;

        public async void OnNavigatedTo()
        {
            _subjectService = new SubjectService();
            _articleService = new ArticleService();

            Subjects = await _subjectService.GetAllSubjectsAcync();

            if (SelectedSubject != null) { Articles = await _articleService.GetAllArticlesAsync(SelectedSubject.Id); }
            else { Articles = await _articleService.GetAllArticlesAsync(); }
        }

        public void OnNavigatedFrom()
        {
            _articleService.Dispose();
            _subjectService.Dispose();
        }

        public async Task UpdateArticlesList()
        {
            if(SelectedSubject != null) Articles = await _articleService.GetAllArticlesAsync(SelectedSubject.Id);
        }
    }
}