using CSharper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharper.Services.AuthenticationServices
{
    public class RegistrationService
    {
        private readonly AppDbContext _context;
        private Encrypt _encrypt;

        public RegistrationService()
        {
            _context = new AppDbContext();
            _encrypt = new Encrypt();
        }

        public async Task<bool> RegistratUser(string login, string password)
        {
            if (string.IsNullOrEmpty(login)) throw new ArgumentException(nameof(login));
            if (string.IsNullOrEmpty(password)) throw new ArgumentException(nameof(password));

            if (_context.Users.Any(u => u.Login == login)) return false;

            string salt = Guid.NewGuid().ToString();
            string hashPassword = await _encrypt.HashPassword(password, salt);
            User user = new User
            {
                Id = Guid.NewGuid(),
                Login = login,
                Password = hashPassword,
                Salt = salt,
                Level = "0"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            AppConfig.User = user;
            return true;
        }
    }
}
