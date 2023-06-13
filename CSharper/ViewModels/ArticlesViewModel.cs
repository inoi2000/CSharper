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
        private IEnumerable<Subject> _subjects;

        public ArticlesViewModel()
        {
            InitializeViewModel();
        }

        private void InitializeViewModel()
        {
        }

        public async void OnNavigatedTo()
        {
            _subjectService = new SubjectService();
            _articleService = new ArticleService();

            Subjects = await _subjectService.GetAllSubjectsAcync();
            Articles = await _articleService.GetAllArticlesAsync();
        }

        public void OnNavigatedFrom()
        {
            _articleService.Dispose();
        }

        // old code, not mine
        [ObservableProperty]
        private int _counter = 0;

        [RelayCommand]
        private void OnCounterIncrement()
        {
            Counter++;
        }
    }
}