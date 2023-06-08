using CSharper.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var book = await _context.Books.FirstAsync(b => b.Id == id);
            return book;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _context.Books.ToListAsync();
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
            var originalBook= await _context.Books.FirstAsync(b => b.Id == originalBookId);

            originalBook.Name = modifiedBook.Name;
            originalBook.Description = modifiedBook.Description;
            originalBook.Experience = modifiedBook.Experience;
            originalBook.LocalLink = modifiedBook.LocalLink;
            originalBook.Url = modifiedBook.Url;
            originalBook.Complexity = modifiedBook.Complexity;
            originalBook.Subject = modifiedBook.Subject;

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

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
