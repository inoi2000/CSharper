using CSharper.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharper.Services
{
    public class LessonService : IDisposable
    {
        private readonly AppDbContext _context;

        public LessonService()
        {
            _context = new AppDbContext();
        }

        public async Task<Lesson> GetLessonAsync(Guid id)
        {
            var lesson = await _context.Lessons.Include(l => l.Subject).FirstAsync(l => l.Id == id);
            return lesson;
        }

        public async Task<IEnumerable<Lesson>> GetAllLessonsAsync()
        {
            return await _context.Lessons.Include(l => l.Subject).ToListAsync();
        }

        public async Task<IEnumerable<Lesson>> GetAllLessonsAsync(Guid subjectId)
        {
            return (await _context.Subjects.Include(s => s.Lessons).FirstAsync(s => s.Id == subjectId))
                .Lessons.ToList();
        }

        public async Task<bool> AddLesson(Lesson lesson)
        {
            if (lesson.Subject == null) { throw new ArgumentNullException(nameof(lesson.Subject)); }
            
            var tempSubject = await _context.Subjects.FirstAsync(s => s.Id == lesson.Subject.Id);
            lesson.Subject = tempSubject;
            
            await _context.Lessons.AddAsync(lesson);

            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<bool> AddLesson(Lesson lesson, Guid subjectId)
        {
            var subject = await _context.Subjects.Include(s => s.Lessons).FirstAsync(s => s.Id == subjectId);
            await _context.Lessons.AddAsync(lesson);
            subject.Lessons.Add(lesson);

            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<bool> RemoveLesson(Lesson lesson)
        {
            _context.Lessons.Remove(lesson);
            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<bool> EditLesson(Lesson modifiedLesson, Guid originalLessonId)
        {
            var originalLesson = await _context.Lessons.FirstAsync(l => l.Id == originalLessonId);
            
            if (originalLesson.Name != modifiedLesson.Name) originalLesson.Name = modifiedLesson.Name;
            if (originalLesson.Description != modifiedLesson.Description) originalLesson.Description = modifiedLesson.Description;
            if (originalLesson.Experience != modifiedLesson.Experience) originalLesson.Experience = modifiedLesson.Experience;
            if (originalLesson.Url != modifiedLesson.Url) originalLesson.Url = modifiedLesson.Url;
            if (originalLesson.Complexity != modifiedLesson.Complexity) originalLesson.Complexity = modifiedLesson.Complexity;

            if (originalLesson.Subject.Id != modifiedLesson.Subject.Id)
            {
                originalLesson.Subject = await _context.Subjects.FirstAsync(s => s.Id == modifiedLesson.Subject.Id);
            }

            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }


        public async Task<bool> IsAccomplitLessonAsync(Guid userId, Guid lessonId)
        {
            var tempUser = await _context.Users.Include(u => u.Lessons).FirstAsync(u => u.Id == userId);
            var tempLesson = await _context.Lessons.FirstAsync(l => l.Id == lessonId);

            return tempUser.Lessons.Contains(tempLesson);
        }

        public async Task<bool> AccomplitLessonAsync(Guid userId, Guid lessonId)
        {
            var tempUser = await _context.Users.Include(u => u.Lessons).FirstAsync(u => u.Id == userId);
            var tempLesson = await _context.Lessons.FirstAsync(l => l.Id == lessonId);

            tempUser.Lessons.Add(tempLesson);

            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<bool> CancelAccomplitLessonAsync(Guid userId, Guid lessonId)
        {
            var tempUser = await _context.Users.Include(u => u.Lessons).FirstAsync(u => u.Id == userId);
            var tempLesson = await _context.Lessons.FirstAsync(l => l.Id == lessonId);

            tempUser.Lessons.Remove(tempLesson);

            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<bool> DownloadLessonAsync(Guid lessonId, IProgress<double> progress, CancellationToken token)
        {
            var lesson = await _context.Books.FirstAsync(b => b.Id == lessonId);

            if (!string.IsNullOrEmpty(lesson.LocalLink) && File.Exists(lesson.LocalLink)) { return true; }

            using (var _downloadingService = new DownloadingService())
            {
                if (lesson.Url != null)
                {
                    string fileName = $"{lesson.Id.ToString()}.pdf";
                    try
                    {
                        await _downloadingService.DownloadToFileAsync(lesson.Url, fileName, progress, token);
                    }
                    catch (OperationCanceledException) { return false; }
                    lesson.LocalLink = $"{_downloadingService.OutPutDirectory}\\{fileName}";
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
