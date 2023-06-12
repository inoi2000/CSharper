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
            var article = await _context.Articles.Include(a => a.Subject).FirstAsync(a => a.Id == id);
            return article;
        }

        public async Task<IEnumerable<Article>> GetAllArticlesAsync()
        {
            return await _context.Articles.Include(a => a.Subject).ToListAsync();
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

        public async Task<bool> RemoveArticle(Article article)
        {
            _context.Articles.Remove(article);
            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<bool> EditArticle(Article modifiedArticle, Guid originalArticleId)
        {
            var originalArticle = await _context.Articles.FirstAsync(a => a.Id == originalArticleId);

            if(originalArticle.Name != modifiedArticle.Name) originalArticle.Name = modifiedArticle.Name;
            if(originalArticle.Description != modifiedArticle.Description) originalArticle.Description = modifiedArticle.Description;
            if (originalArticle.Experience != modifiedArticle.Experience) originalArticle.Experience = modifiedArticle.Experience;
            if (originalArticle.Url != modifiedArticle.Url) originalArticle.Url = modifiedArticle.Url;
            if (originalArticle.Complexity != modifiedArticle.Complexity) originalArticle.Complexity = modifiedArticle.Complexity;

            if (originalArticle.Subject.Id != modifiedArticle.Subject.Id)
            {
                originalArticle.Subject = await _context.Subjects.FirstAsync(s => s.Id == modifiedArticle.Subject.Id);
            }

            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<bool> IsAccomplitArticleAsync(Guid userId, Guid articleId)
        {
            var tempUser = await _context.Users.Include(u => u.Articles).FirstAsync(u => u.Id == userId);
            var tempArticle = await _context.Articles.FirstAsync(a => a.Id == articleId);

            return tempUser.Articles.Contains(tempArticle);
        }

        public async Task<bool> AccomplitArticleAsync(Guid userId, Guid articleId)
        {
            var tempUser = await _context.Users.Include(u => u.Articles).FirstAsync(u => u.Id == userId);
            var tempArticle = await _context.Articles.FirstAsync(a => a.Id == articleId);

            tempUser.Articles.Add(tempArticle);

            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<bool> CancelAccomplitArticleAsync(Guid userId, Guid articleId)
        {
            var tempUser = await _context.Users.Include(u => u.Articles).FirstAsync(u => u.Id == userId);
            var tempArticle = await _context.Articles.FirstAsync(a => a.Id == articleId);

            tempUser.Articles.Remove(tempArticle);

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
