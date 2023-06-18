using CSharper.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharper.Services.AuthenticationServices
{
    public class AuthorizationService
    {
        private readonly AppDbContext _context;
        private Encrypt _encrypt;

        public AuthorizationService()
        {
            _context = new AppDbContext();
            _encrypt = new Encrypt();
        }

        public async Task<bool> Login(string login, string password)
        {
            if(string.IsNullOrEmpty(login)) throw new ArgumentException(nameof(login));
            if(string.IsNullOrEmpty(password)) throw new ArgumentException(nameof(password));

            User user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
            if (user == null) return false;

            if (user.Password == await _encrypt.HashPassword(password, user.Salt))
            {
                AppConfig.User = user;
            }
            return true;
        }

    }
}
