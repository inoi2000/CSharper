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
    public partial class AddArticleViewModel : ObservableObject, INavigationAware
    {
        public IEnumerable<Complexity> Complexities => Enum.GetValues(typeof(Complexity)).Cast<Complexity>();
        private ArticleService _articleService { get; set; }
        private SubjectService _subjectService { get; set; }


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddNewArticleCommand))]
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
        private IEnumerable<Article> _articles;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddNewArticleCommand))]
        private Subject _selectedSubject;

        [ObservableProperty]
        private Complexity _selectedComplexity;

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

        [RelayCommand(CanExecute = nameof(CanAddNewArticle))]
        private async Task AddNewArticle()
        {
            var article = new Article()
            {
                Id = Guid.NewGuid(),
                Name = this.Name,
                Description = this.Description,
                Complexity = SelectedComplexity,
                Experience = Experience,
                Url = new Uri(Uri),
            };

            if (await _articleService.AddArticle(article, SelectedSubject.Id))
            {
                Name = string.Empty;
                Description = string.Empty;
                Uri = string.Empty;

                Articles = await _articleService.GetAllArticlesAsync();
            }
        }

        private bool CanAddNewArticle()
        {
            return !string.IsNullOrWhiteSpace(Name) && SelectedSubject != null;
        }
    }
}
