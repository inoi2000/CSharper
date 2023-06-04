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

        public async Task<IEnumerable<Video>> GetAllVideosAsync(Guid subjectId)
        {
            return (await _context.Subjects.Include(s => s.Videos).FirstAsync(s => s.Id == subjectId))
                .Videos.ToList();
        }

        public async Task<bool> AddVideo(Subject subject, Video video)
        {
            (await _context.Subjects.Include(s => s.Videos).FirstAsync(s => s.Equals(subject)))
                .Videos.Add(video);
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

        public async Task<bool> EditVideo(Video originalVideo, Video modifiedVideo)
        {
            var tempVideo = await _context.Lessons.FirstAsync(v => v.Equals(originalVideo));

            tempVideo.Name = modifiedVideo.Name;
            tempVideo.Description = modifiedVideo.Description;
            tempVideo.Experience = modifiedVideo.Experience;
            tempVideo.LocalLink = modifiedVideo.LocalLink;
            tempVideo.Url = modifiedVideo.Url;

            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<bool> IsAccomplitVideoAsync(User user, Video video)
        {
            var tempUser = await _context.Users.Include(u => u.Videos).FirstAsync(u => u.Equals(user));
            var tempVideo = await _context.Videos.FirstAsync(l => l.Equals(video));

            return tempUser.Videos.Contains(tempVideo);
        }

        public async Task<bool> AccomplitVideoAsync(User user, Video video)
        {
            var tempUser = await _context.Users.Include(u => u.Videos).FirstAsync(u => u.Equals(user));
            var tempVideo = await _context.Videos.FirstAsync(l => l.Equals(video));

            tempUser.Videos.Add(tempVideo);

            int count = await _context.SaveChangesAsync();
            if (count > 0) { return true; }
            else { return false; }
        }

        public async Task<bool> CancelAccomplitVideoAsync(User user, Video video)
        {
            var tempUser = await _context.Users.Include(u => u.Videos).FirstAsync(u => u.Equals(user));
            var tempVideo = await _context.Videos.FirstAsync(l => l.Equals(video));

            tempUser.Videos.Remove(video);

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
