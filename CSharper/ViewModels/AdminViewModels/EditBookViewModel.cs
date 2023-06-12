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
    public partial class EditBookViewModel : ObservableObject, INavigationAware
    {
        public IEnumerable<Complexity> Complexities => Enum.GetValues(typeof(Complexity)).Cast<Complexity>();
        private BookService _bookService { get; set; }
        private SubjectService _subjectService { get; set; }


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditBookCommand))]
        private string _name = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditBookCommand))]
        private string? _description = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditBookCommand))]
        private double _experience = 0D;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditBookCommand))]
        private string _uri = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditBookCommand))]
        private Complexity _complexity = 0;

        [ObservableProperty]
        private IEnumerable<Subject> _subjects;

        [ObservableProperty]
        private IEnumerable<Book> _books;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditBookCommand))]
        private Subject _selectedSubject;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditBookCommand))]
        private Complexity _selectedComplexity;


        private Book _selectedBook;
        public Book SelectedBook
        {
            get
            {
                return _selectedBook;
            }
            set
            {
                _selectedBook = value;
                OnPropertyChanged(nameof(SelectedBook));
                if (SelectedBook is Book book)
                {
                    Name = book.Name;
                    Description = book.Description;
                    Experience = book.Experience;
                    Uri = book.Url?.ToString() ?? string.Empty;
                    SelectedComplexity = book.Complexity;
                    SelectedSubject = Subjects?.FirstOrDefault(s => s.Id == book.Subject.Id);
                }
            }
        }

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

        [RelayCommand(CanExecute = nameof(CanEditBook))]
        private async Task EditBook()
        {
            var book = new Book()
            {
                Id = Guid.NewGuid(),
                Name = this.Name,
                Description = this.Description,
                Complexity = SelectedComplexity,
                Experience = Experience,
                Url = new Uri(Uri),
                Subject = SelectedSubject
            };

            await _bookService.EditBook(book, SelectedBook.Id);
            Books = await _bookService.GetAllBooksAsync();
        }

        private bool CanEditBook()
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                !string.IsNullOrWhiteSpace(Uri) &&
                SelectedBook != null &&
               (SelectedBook.Name != this.Name ||
                SelectedBook.Description != this.Description ||
                SelectedBook.Complexity != this.SelectedComplexity ||
                SelectedBook.Experience != this.Experience ||
                SelectedBook.Url?.ToString() != this.Uri ||
                SelectedBook.Subject.Id != SelectedSubject?.Id);
        }
    }
}
