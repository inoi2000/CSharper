using CSharper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharper.Services
{
    public class VideoService : IDisposable
    {
        private readonly AppDbContext _context;

        public VideoService()
        {
            _context = new AppDbContext();
        }

        public async Task<Video> GetVideoAsync(Guid id)
        {
            var video = await _context.Videos.FirstAsync(v => v.Id == id);
            return video;
        }

        public async Task<IEnumerable<Video>> GetAllVideosAsync()
        {
            return await _context.Videos.ToListAsync();
        }

        public async Task<IEnumerable<Video>> GetAllVideosAsync(Guid subjectId)
        {
            return (await _context.Subjects.Include(s => s.Videos).FirstAsync(s => s.Id == subjectId))
                .Videos.ToList();
        }

        public async Task<bool> AddVideo(Video video)
        {
            if (video.Subject == null) { throw new ArgumentNullException(nameof(video.Subject)); }
            await _context.Videos.AddAsync(video);

            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<bool> AddVideo(Video video, Guid subjectId)
        {
            var subject = await _context.Subjects.Include(s => s.Videos).FirstAsync(s => s.Id == subjectId);
            await _context.Videos.AddAsync(video);
            subject.Videos.Add(video);

            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<bool> RemoveVideo(Video video)
        {
            _context.Videos.Remove(video);
            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<bool> EditVideo(Video modifiedVideo, Guid originalVideoId)
        {
            var originalVideo = await _context.Videos.FirstAsync(v => v.Id == originalVideoId);

            originalVideo.Name = modifiedVideo.Name;
            originalVideo.Description = modifiedVideo.Description;
            originalVideo.Experience = modifiedVideo.Experience;
            originalVideo.LocalLink = modifiedVideo.LocalLink;
            originalVideo.Url = modifiedVideo.Url;
            originalVideo.Complexity = modifiedVideo.Complexity;
            originalVideo.Subject = modifiedVideo.Subject;

            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<bool> IsAccomplitVideoAsync(Guid userId, Guid videoId)
        {
            var tempUser = await _context.Users.Include(u => u.Videos).FirstAsync(u => u.Id == userId);
            var tempVideo = await _context.Videos.FirstAsync(v => v.Id == videoId);

            return tempUser.Videos.Contains(tempVideo);
        }

        public async Task<bool> AccomplitVideoAsync(Guid userId, Guid videoId)
        {
            var tempUser = await _context.Users.Include(u => u.Videos).FirstAsync(u => u.Id == userId);
            var tempVideo = await _context.Videos.FirstAsync(v => v.Id == videoId);

            tempUser.Videos.Add(tempVideo);

            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<bool> CancelAccomplitVideoAsync(Guid userId, Guid videoId)
        {
            var tempUser = await _context.Users.Include(u => u.Videos).FirstAsync(u => u.Id == userId);
            var tempVideo = await _context.Videos.FirstAsync(v => v.Id == videoId);

            tempUser.Videos.Remove(tempVideo);

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
