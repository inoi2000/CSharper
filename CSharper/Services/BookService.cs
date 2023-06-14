using CSharper.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharper.Services
{
    public class BookService : IDisposable
    {
        private readonly AppDbContext _context;

        public BookService()
        {
            _context = new AppDbContext();
        }

        public async Task<Book> GetBookAsync(Guid id)
        {
            var book = await _context.Books.Include(b => b.Subject).FirstAsync(b => b.Id == id);
            return book;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _context.Books.Include(b => b.Subject).Include(b => b.Users).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync(Guid subjectId)
        {
            return (await _context.Subjects.Include(s => s.Books).FirstAsync(s => s.Id == subjectId))
                .Books.ToList();
        }

        public async Task<bool> AddBook(Book book)
        {
            if(book.Subject == null) { throw new ArgumentNullException(nameof(book.Subject)); }
            await _context.Books.AddAsync(book);

            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<bool> AddBook(Book book, Guid subjectId)
        {
            var subject = await _context.Subjects.Include(s => s.Books).FirstAsync(s => s.Id == subjectId);
            await _context.Books.AddAsync(book);
            subject.Books.Add(book);

            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<bool> RemoveBook(Book book)
        {
            _context.Books.Remove(book);
            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<bool> EditBook(Book modifiedBook, Guid originalBookId)
        {
            var originalBook = await _context.Books.FirstAsync(b => b.Id == originalBookId);

            if (originalBook.Name != modifiedBook.Name) originalBook.Name = modifiedBook.Name;
            if (originalBook.Description != modifiedBook.Description) originalBook.Description = modifiedBook.Description;
            if (originalBook.Experience != modifiedBook.Experience) originalBook.Experience = modifiedBook.Experience;
            if (originalBook.Url != modifiedBook.Url) originalBook.Url = modifiedBook.Url;
            if (originalBook.Complexity != modifiedBook.Complexity) originalBook.Complexity = modifiedBook.Complexity;

            if (originalBook.Subject.Id != modifiedBook.Subject.Id)
            {
                originalBook.Subject = await _context.Subjects.FirstAsync(s => s.Id == modifiedBook.Subject.Id);
            }

            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<bool> IsAccomplitBookAsync(Guid userId, Guid bookId)
        {
            var tempUser = await _context.Users.Include(u => u.Books).FirstAsync(u => u.Id == userId);
            var tempBook = await _context.Books.FirstAsync(b => b.Id == bookId);

            return tempUser.Books.Contains(tempBook);
        }

        public bool IsAccomplitBook(Guid userId, Guid bookId)
        {
            var tempUser = _context.Users.Include(u => u.Books).First(u => u.Id == userId);
            var tempBook = _context.Books.First(b => b.Id == bookId);

            return tempUser.Books.Contains(tempBook);
        }

        public async Task<bool> AccomplitBookAsync(Guid userId, Guid bookId)
        {
            var tempUser = await _context.Users.Include(u => u.Books).FirstAsync(u => u.Id == userId);
            var tempBook = await _context.Books.FirstAsync(b => b.Id == bookId);

            tempUser.Books.Add(tempBook);

            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<bool> CancelAccomplitBookAsync(Guid userId, Guid bookId)
        {
            var tempUser = await _context.Users.Include(u => u.Books).FirstAsync(u => u.Id == userId);
            var tempBook = await _context.Books.FirstAsync(b => b.Id == bookId);

            tempUser.Books.Remove(tempBook);

            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }


        public async Task<Stream> OpenBookAsync(Guid bookId)
        {
            var book = await _context.Books.FirstAsync(b => b.Id == bookId);
            using (var _downloadingService = new DownloadingService())
            {
                return await _downloadingService.DownloadToMemoryAsync(book.Url);
            }
        }

        public async Task<bool> DownloadBookAsync(Guid bookId, IProgress<double> progress, CancellationToken token)
        {
            var book = await _context.Books.FirstAsync(b => b.Id == bookId);

            if(!string.IsNullOrEmpty(book.LocalLink) && File.Exists(book.LocalLink)) { return true; }

            using (var _downloadingService = new DownloadingService())
            {
                if (book.Url != null)
                {
                    string fileName = $"{book.Id.ToString()}.pdf";
                    try
                    {
                        await _downloadingService.DownloadToFileAsync(book.Url, fileName, progress, token);
                    }
                    catch (OperationCanceledException) { return false; }
                    book.LocalLink = $"{_downloadingService.OutPutDirectory}\\{fileName}";
                }
                else { return false; }
            }

            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
