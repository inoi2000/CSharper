using CSharper.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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
            var lesson = await _context.Lessons.FirstAsync(l => l.Id == id);
            return lesson;
        }

        public async Task<IEnumerable<Lesson>> GetAllLessonsAsync(Guid subjectId)
        {
            return (await _context.Subjects.Include(s => s.Lessons).FirstAsync(s => s.Id == subjectId))
                .Lessons.ToList();
        }

        public async Task<bool> AddLesson(Subject subject, Lesson lesson)
        {
            (await _context.Subjects.Include(s => s.Lessons).FirstAsync(s => s.Equals(subject)))
                .Lessons.Add(lesson);
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

        public async Task<bool> EditLesson(Lesson originalLesson, Lesson modifiedLesson)
        {
            var tempLesson = await _context.Lessons.FirstAsync(l => l.Equals(originalLesson));

            tempLesson.Name = modifiedLesson.Name;
            tempLesson.Description = modifiedLesson.Description;
            tempLesson.Experience = modifiedLesson.Experience;
            tempLesson.LocalLink = modifiedLesson.LocalLink;
            tempLesson.Url = modifiedLesson.Url;

            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }


        public async Task<bool> IsAccomplitLessonAsync(User user, Lesson lesson)
        {
            var tempUser = await _context.Users.Include(u => u.Lessons).FirstAsync(u => u.Equals(user));
            var tempLesson = await _context.Lessons.FirstAsync(l => l.Equals(lesson));

            return tempUser.Lessons.Contains(tempLesson);
        }

        public async Task<bool> AccomplitLessonAsync(User user, Lesson lesson)
        {
            var tempUser = await _context.Users.Include(u => u.Lessons).FirstAsync(u => u.Equals(user));
            var tempLesson = await _context.Lessons.FirstAsync(l => l.Equals(lesson));

            tempUser.Lessons.Add(lesson);

            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<bool> CancelAccomplitLessonAsync(User user, Lesson lesson)
        {
            var tempUser = await _context.Users.Include(u => u.Lessons).FirstAsync(u => u.Equals(user));
            var tempLesson = await _context.Lessons.FirstAsync(l => l.Equals(lesson));

            tempUser.Lessons.Remove(lesson);

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
