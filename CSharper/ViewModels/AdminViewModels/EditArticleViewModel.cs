using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSharper.Models;
using CSharper.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Common.Interfaces;

namespace CSharper.ViewModels.AdminViewModels
{
    public partial class EditArticleViewModel : ObservableObject, INavigationAware
    {
        public IEnumerable<Complexity> Complexities => Enum.GetValues(typeof(Complexity)).Cast<Complexity>();
        private ArticleService _articleService { get; set; }
        private SubjectService _subjectService { get; set; }


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditArticleCommand))]
        private string _name = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditArticleCommand))]
        private string? _description = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditArticleCommand))]
        private double _experience = 0D;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditArticleCommand))]
        private string _uri = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditArticleCommand))]
        private Complexity _complexity = 0;

        [ObservableProperty]
        private IEnumerable<Subject> _subjects;

        [ObservableProperty]
        private IEnumerable<Article> _articles;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditArticleCommand))]
        private Subject _selectedSubject;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditArticleCommand))]
        private Complexity _selectedComplexity;

        
        private Article _selectedAticle;
        public Article SelectedAticle
        {
            get
            {
                return _selectedAticle;
            }
            set
            {
                _selectedAticle = value;
                OnPropertyChanged(nameof(SelectedAticle));
                if (SelectedAticle is Article article)
                {
                    Name = article.Name;
                    Description = article.Description;
                    Experience = article.Experience;
                    Uri = article.Url?.ToString() ?? string.Empty;
                    SelectedComplexity = article.Complexity;
                    SelectedSubject = Subjects?.FirstOrDefault(s => s.Id == article.Subject.Id);
                }
            }
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
            _articleService?.Dispose();
            _subjectService?.Dispose();
        }

        [RelayCommand(CanExecute = nameof(CanEditArticle))]
        private async Task EditArticle()
        {
            var article = new Article()
            {
                Id = Guid.NewGuid(),
                Name = this.Name,
                Description = this.Description,
                Complexity = SelectedComplexity,
                Experience = Experience,
                Url = new Uri(Uri),
                Subject = SelectedSubject
            };

            await _articleService.EditArticle(article, SelectedAticle.Id);
            Articles = await _articleService.GetAllArticlesAsync();
        }

        private bool CanEditArticle()
        {
            return !string.IsNullOrWhiteSpace(Name) && 
                !string.IsNullOrWhiteSpace(Uri) &&
                SelectedAticle != null &&
               (SelectedAticle.Name != this.Name ||
                SelectedAticle.Description != this.Description || 
                SelectedAticle.Complexity != this.SelectedComplexity || 
                SelectedAticle.Experience != this.Experience ||
                SelectedAticle.Url?.ToString() != this.Uri ||
                SelectedAticle.Subject.Id != SelectedSubject?.Id);
        }
    }
}
