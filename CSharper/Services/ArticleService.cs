using CSharper.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharper.Services
{
    public class ArticleService
    {
        private readonly AppDbContext _context;

        public ArticleService()
        {
            _context = new AppDbContext();
        }

        public async Task<Article> GetArticleAsync(Guid id)
        {
            var article = await _context.Articles.FirstAsync(a => a.Id == id);
            return article;
        }

        public async Task<IEnumerable<Article>> GetAllArticlesAsync(Guid subjectId)
        {
            return (await _context.Subjects.Include(s => s.Articles).FirstAsync(s => s.Id == subjectId))
                .Articles.ToList();
        }

        public async Task<bool> AddArticle(Article article)
        {
            if (article.Subject == null) { throw new ArgumentNullException(nameof(article.Subject)); }
            await _context.Articles.AddAsync(article);

            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<bool> AddArticle(Article article, Guid subjectId)
        {
            var subject = await _context.Subjects.Include(s => s.Articles).FirstAsync(s => s.Id == subjectId);
            await _context.Articles.AddAsync(article);
            subject.Articles.Add(article);

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
