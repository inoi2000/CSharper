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
    public partial class AddBookViewModel : ObservableObject, INavigationAware
    {
        public IEnumerable<Complexity> Complexities => Enum.GetValues(typeof(Complexity)).Cast<Complexity>();
        private BookService _bookService { get; set; }
        private SubjectService _subjectService { get; set; }


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddNewBookCommand))]
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
        private IEnumerable<Book> _books;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddNewBookCommand))]
        private Subject _selectedSubject;

        [ObservableProperty]
        private Complexity _selectedComplexity;

        public async void OnNavigatedTo()
        {
            _subjectService = new SubjectService();
            _bookService = new BookService();

            Subjects = await _subjectService.GetAllSubjectsAcync();
            Books = await _bookService.GetAllBooksAsync();
        }

        public void OnNavigatedFrom()
        {
            _bookService?.Dispose();
            _subjectService?.Dispose();
        }

        [RelayCommand(CanExecute = nameof(CanAddNewBook))]
        private async Task AddNewBook()
        {
            var book = new Book()
            {
                Id = Guid.NewGuid(),
                Name = this.Name,
                Description = this.Description,
                Complexity = SelectedComplexity,
                Experience = Experience,
                Url = new Uri(Uri),
            };

            if (await _bookService.AddBook(book, SelectedSubject.Id))
            {
                Name = string.Empty;
                Description = string.Empty;
                Uri = string.Empty;

                Books = await _bookService.GetAllBooksAsync();
            }
        }

        private bool CanAddNewBook()
        {
            return !string.IsNullOrWhiteSpace(Name) && SelectedSubject != null;
        }
    }
}
