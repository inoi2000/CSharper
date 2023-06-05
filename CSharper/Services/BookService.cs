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

        public async Task<IEnumerable<Book>> GetAllBooksAsync(Guid subjectId)
        {
            return (await _context.Subjects.Include(s => s.Books).FirstAsync(s => s.Id == subjectId))
                .Books.ToList();
        }

        public async Task<bool> AddBook(Guid subjectId, Book book)
        {
            var tempSubject = await _context.Subjects.Include(s => s.Books).FirstAsync(s => s.Id == subjectId);
            await _context.Books.AddAsync(book);
            tempSubject.Books.Add(book);

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

        public async Task<bool> EditBook(Book originalBook, Book modifiedBook)
        {
            var tempBook = await _context.Books.FirstAsync(b => b.Equals(originalBook));

            tempBook.Name = modifiedBook.Name;
            tempBook.Description = modifiedBook.Description;
            tempBook.Experience = modifiedBook.Experience;
            tempBook.LocalLink = modifiedBook.LocalLink;
            tempBook.Url = modifiedBook.Url;

            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<bool> IsAccomplitBookAsync(User user, Book book)
        {
            var tempUser = await _context.Users.Include(u => u.Books).FirstAsync(u => u.Equals(user));
            var tempBook = await _context.Books.FirstAsync(b => b.Equals(book));

            return tempUser.Books.Contains(tempBook);
        }

        public async Task<bool> AccomplitBookAsync(User user, Book book)
        {
            var tempUser = await _context.Users.Include(u => u.Books).FirstAsync(u => u.Equals(user));
            var tempBook = await _context.Books.FirstAsync(b => b.Equals(book));

            tempUser.Books.Add(tempBook);

            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<bool> CancelAccomplitBookAsync(User user, Book book)
        {
            var tempUser = await _context.Users.Include(u => u.Books).FirstAsync(u => u.Equals(user));
            var tempBook = await _context.Books.FirstAsync(b => b.Equals(book));

            tempUser.Books.Remove(book);

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
