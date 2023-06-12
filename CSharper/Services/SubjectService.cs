using CSharper.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharper.Services
{
    public class SubjectService : IDisposable
    {
        private readonly AppDbContext _context;

        public SubjectService()
        {
            _context = new AppDbContext();
        }

        public async Task<Subject> GetSubjectAcync(Guid id)
        {
            var subject = await _context.Subjects.FirstAsync(s =>  s.Id == id);
            return subject;
        }

        public async Task<IEnumerable<Subject>> GetAllSubjectsAcync()
        {
            var subjects = await _context.Subjects.ToListAsync();
            return subjects;
        }

        /// <summary>
        /// Асинхронный метод осуществляющий ленивую загрузку из BD всех учебных материалов по выбраному предмету
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        public async Task LoadSubjectStudyMaterialAcync(Subject subject)
        {
            await _context.Entry(subject).Collection(s => s.Lessons).LoadAsync();
            await _context.Entry(subject).Collection(s => s.Books).LoadAsync();
            await _context.Entry(subject).Collection(s => s.Videos).LoadAsync();
            await _context.Entry(subject).Collection(s => s.Articles).LoadAsync();
            await _context.Entry(subject).Collection(s => s.Assignments).LoadAsync();
        }


        public async Task<bool> AddSubject(Subject subject)
        {
            await _context.Subjects.AddAsync(subject);
            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<bool> EditSubject(Subject modifiedSubject, Guid originalSubjectId)
        {
            var originalSubject = await _context.Subjects.FirstAsync(s => s.Id == originalSubjectId);

            if (originalSubject.Name != modifiedSubject.Name) originalSubject.Name = modifiedSubject.Name;
            if (originalSubject.Description != modifiedSubject.Description) originalSubject.Description = modifiedSubject.Description;
            if (originalSubject.Complexity != modifiedSubject.Complexity) originalSubject.Complexity = modifiedSubject.Complexity;

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
