using CSharper.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        // временный метод для создания тестового пользователя
        public async Task CreateUser()
        {
            var user = new User 
            { 
                Id = Guid.NewGuid(),
                Login = "TestUser",
                Password = "password",
                Level = "1",
                Experience = 100D
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        // временный мето для получения тестового пользователя
        public User GetTestUser()
        {
            var user =  _context.Users.First(u => u.Login == "TestUser");
            return user;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
