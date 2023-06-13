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
        public string? Title { get; set; } // Delete maybe, don't know why it's here

        private ArticleService _articleService;

        [ObservableProperty]
        private IEnumerable<Article> _articles;

        public ArticlesViewModel()
        {
            InitializeViewModel();
        }

        private void InitializeViewModel()
        {
        }

        public async void OnNavigatedTo()
        {
            _articleService = new ArticleService();
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