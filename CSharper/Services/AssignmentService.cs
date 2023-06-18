﻿using CSharper.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharper.Services
{
    public class AssignmentService : IDisposable
    {
        private readonly AppDbContext _context;

        public AssignmentService()
        {
            _context = new AppDbContext();
        }

        public async Task<Assignment> GetAssignmentAsync(Guid id)
        {
            var assignment = await _context.Assignments.Include(a => a.Subject).FirstAsync(b => b.Id == id);
            return assignment;
        }

        public async Task<IEnumerable<Assignment>> GetAllAssignmentsAsync()
        {
            return await _context.Assignments.Include(a => a.Subject).ToListAsync();
        }

        public async Task<IEnumerable<Assignment>> GetAllAssignmentsAsync(Guid subjectId)
        {
            return (await _context.Subjects.Include(s => s.Assignments).FirstAsync(s => s.Id == subjectId))
                .Assignments.ToList();
        }

        public async Task<bool> AddAssignment(Assignment assignment)
        {
            if (assignment.Subject == null) { throw new ArgumentNullException(nameof(assignment.Subject)); }
            await _context.Assignments.AddAsync(assignment);

            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<bool> AddAssignment(Assignment assignment, Guid subjectId)
        {
            var subject = await _context.Subjects.Include(s => s.Assignments).FirstAsync(s => s.Id == subjectId);
            await _context.Assignments.AddAsync(assignment);
            subject.Assignments.Add(assignment);

            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<bool> RemoveAssignment(Assignment assignment)
        {
            _context.Assignments.Remove(assignment);
            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<bool> EditAssignment(Assignment modifiedAssignment, Guid originalAssignmentId)
        {
            var originalAssignment = await _context.Assignments.FirstAsync(a => a.Id == originalAssignmentId);
            
            if (originalAssignment.Name != modifiedAssignment.Name) originalAssignment.Name = modifiedAssignment.Name;
            if (originalAssignment.Description != modifiedAssignment.Description) originalAssignment.Description = modifiedAssignment.Description;
            if (originalAssignment.Experience != modifiedAssignment.Experience) originalAssignment.Experience = modifiedAssignment.Experience;
            if (originalAssignment.Url != modifiedAssignment.Url) originalAssignment.Url = modifiedAssignment.Url;
            if (originalAssignment.Complexity != modifiedAssignment.Complexity) originalAssignment.Complexity = modifiedAssignment.Complexity;

            if (originalAssignment.Subject.Id != modifiedAssignment.Subject.Id)
            {
                originalAssignment.Subject = await _context.Subjects.FirstAsync(s => s.Id == modifiedAssignment.Subject.Id);
            }

            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<bool> IsAccomplitAssignmentAsync(Guid userId, Guid assignmentId)
        {
            var tempUser = await _context.Users.Include(u => u.Assignments).FirstAsync(u => u.Id == userId);
            var tempAssignment = await _context.Assignments.FirstAsync(a => a.Id == assignmentId);

            return tempUser.Assignments.Contains(tempAssignment);
        }

        public async Task<bool> AccomplitAssignmentAsync(Guid userId, Guid assignmentId)
        {
            var tempUser = await _context.Users.Include(u => u.Assignments).FirstAsync(u => u.Id == userId);
            var tempAssignment = await _context.Assignments.FirstAsync(a => a.Id == assignmentId);

            tempUser.Assignments.Add(tempAssignment);

            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<bool> CancelAccomplitAssignmentAsync(Guid userId, Guid assignmentId)
        {
            var tempUser = await _context.Users.Include(u => u.Assignments).FirstAsync(u => u.Id == userId);
            var tempAssignment = await _context.Assignments.FirstAsync(a => a.Id == assignmentId);

            tempUser.Assignments.Remove(tempAssignment);

            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<Stream> OpenAssignmentAsync(Guid assignmentId)
        {
            var assignment = await _context.Assignments.FirstAsync(a => a.Id == assignmentId);
            using (var _downloadingService = new DownloadingService())
            {
                return await _downloadingService.DownloadToMemoryAsync(assignment.Url);
            }
        }

        public async Task<bool> DownloadAssignmentAsync(Guid assignmentId, IProgress<double> progress, CancellationToken token)
        {
            var assignment = await _context.Assignments.FirstAsync(a => a.Id == assignmentId);

            if (!string.IsNullOrEmpty(assignment.LocalLink) && File.Exists(assignment.LocalLink)) { return true; }

            using (var _downloadingService = new DownloadingService())
            {
                if (assignment.Url != null)
                {
                    string fileName = $"{assignment.Id.ToString()}.pdf";
                    try
                    {
                        await _downloadingService.DownloadToFileAsync(assignment.Url, fileName, progress, token);
                    }
                    catch (OperationCanceledException) { return false; }
                    assignment.LocalLink = $"{_downloadingService.OutPutDirectory}\\{fileName}";
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
