using CSharper.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharper.Services
{
    public class UserService : IDisposable
    {
        private readonly AppDbContext _context;

        public UserService()
        {
            _context = new AppDbContext();
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            var lesson = await _context.Users.FirstAsync(u => u.Id == id);
            return lesson;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
